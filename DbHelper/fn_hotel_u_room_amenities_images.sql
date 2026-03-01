CREATE OR REPLACE FUNCTION public.fn_hotel_u_room_amenities_images(p_room_id integer, p_name character varying, p_price_per_night numeric, p_capacity integer, p_description character varying, p_is_working boolean, p_amenities json, p_images json)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    -- 1. Actualizar habitación
    UPDATE rooms
    SET
        name = p_name,
        price_per_night = p_price_per_night,
        capacity = COALESCE(p_capacity, 1),
        description = COALESCE(p_description, ''),
        is_working = p_is_working
    WHERE room_id = p_room_id;

    -- 2. Eliminar amenidades actuales
    DELETE FROM room_amenities
    WHERE room_id = p_room_id;

    -- 3. Insertar nuevas amenidades (FIXED CAST)
    INSERT INTO room_amenities (room_id, amenity_id)
    SELECT
        p_room_id,
        (value)::INTEGER
    FROM json_array_elements_text(p_amenities); -- Usa _text para poder convertir a INTEGER

    -- 4. Eliminar imágenes actuales
    DELETE FROM room_images
    WHERE room_id = p_room_id;

    -- 5. Insertar nuevas imágenes
    INSERT INTO room_images (room_id, image_url)
    SELECT
        p_room_id,
        value::TEXT
    FROM json_array_elements_text(p_images);

END;
$function$
