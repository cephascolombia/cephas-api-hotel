CREATE OR REPLACE FUNCTION public.fn_hotel_c_room_amenities_images(p_name character varying, p_price_per_night numeric, p_created_by character varying, p_capacity integer DEFAULT 1, p_description character varying DEFAULT ''::character varying, p_is_working boolean DEFAULT true, p_amenities integer[] DEFAULT NULL::integer[], p_images text[] DEFAULT NULL::text[])
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE

    v_room_id INTEGER;
    v_amenity INTEGER;
    v_image TEXT;

BEGIN

    -------------------------------------------------
    -- 1. INSERT ROOM
    -------------------------------------------------

    INSERT INTO rooms (
        name,
        price_per_night,
        capacity,
        description,
        is_working,
        created_by
    )
    VALUES (
        p_name,
        p_price_per_night,
        p_capacity,
        p_description,
        p_is_working,
        p_created_by
    )
    RETURNING room_id INTO v_room_id;


    -------------------------------------------------
    -- 2. INSERT AMENITIES
    -------------------------------------------------

    IF p_amenities IS NOT NULL THEN
    
        FOREACH v_amenity IN ARRAY p_amenities
        LOOP
        
            INSERT INTO room_amenities(
                room_id,
                amenity_id
            )
            VALUES(
                v_room_id,
                v_amenity
            );

        END LOOP;

    END IF;


    -------------------------------------------------
    -- 3. INSERT IMAGES
    -------------------------------------------------

    IF p_images IS NOT NULL THEN
    
        FOREACH v_image IN ARRAY p_images
        LOOP
        
            INSERT INTO room_images(
                room_id,
                image_url
            )
            VALUES(
                v_room_id,
                v_image
            );

        END LOOP;

    END IF;


    -------------------------------------------------
    -- RETURN ROOM ID
    -------------------------------------------------

    RETURN v_room_id;

END;
$function$
