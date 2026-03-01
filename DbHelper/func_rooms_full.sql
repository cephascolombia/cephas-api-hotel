CREATE OR REPLACE FUNCTION public.fn_hotel_s_rooms_full(p_page integer, p_page_size integer, p_name character varying, p_capacity integer, p_min_price numeric, p_max_price numeric, p_sort character varying)
 RETURNS TABLE(room_id integer, name character varying, price_per_night numeric, capacity integer, description character varying, is_available boolean, amenities jsonb, images jsonb, total_records integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        r.room_id,
        r.name,
        r.price_per_night,
        r.capacity,
        r.description,
        TRUE AS is_available,
        (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'amenity_id', a.amenity_id,
                    'name', a.name
                )
            )
            FROM room_amenities ra
            JOIN amenities a ON a.amenity_id = ra.amenity_id
            WHERE ra.room_id = r.room_id AND a.is_active = TRUE
        ) AS amenities,
        (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'image_id', ri.image_id,
                    'url', ri.image_url
                )
            )
            FROM room_images ri
            WHERE ri.room_id = r.room_id
        ) AS images,
        COALESCE((COUNT(*) OVER())::INTEGER, 0::INTEGER) AS total_records
    FROM rooms r
    WHERE
        r.is_active = TRUE
        AND (p_name IS NULL OR r.name ILIKE '%' || p_name || '%')
        AND (p_capacity IS NULL OR r.capacity >= p_capacity)
        AND (p_min_price IS NULL OR r.price_per_night >= p_min_price)
        AND (p_max_price IS NULL OR r.price_per_night <= p_max_price)
    ORDER BY
        CASE WHEN p_sort = 'price' THEN r.price_per_night END,
        CASE WHEN p_sort = 'capacity' THEN r.capacity END
    LIMIT p_page_size
    OFFSET (p_page - 1) * p_page_size;
END;
$function$
