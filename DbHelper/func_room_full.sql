CREATE OR REPLACE FUNCTION public.fn_hotel_s_room_full(p_room_id integer)
 RETURNS TABLE(room_id integer, name character varying, price_per_night numeric, capacity integer, description character varying, is_active boolean, created_date timestamp without time zone, created_by character varying, amenities json, images json)
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
    r.is_active,
    r.created_date,
    r.created_by,

    -- Amenidades
    (
        SELECT json_agg(
            json_build_object(
                'amenity_id', a.amenity_id,
                'name', a.name
            )
        )
        FROM room_amenities ra
        JOIN amenities a
            ON a.amenity_id = ra.amenity_id
        WHERE ra.room_id = r.room_id AND a.is_active = TRUE
    ) AS amenities,

    -- Imágenes
    (
        SELECT json_agg(
            json_build_object(
                'image_id', ri.image_id,
                'image_url', ri.image_url
            )
        )
        FROM room_images ri
        WHERE ri.room_id = r.room_id
    ) AS images

FROM rooms r
WHERE r.room_id = p_room_id;

END;
$function$
