



/*   
CREATE OR REPLACE FUNCTION public.test_completes_insert (
        p_test_varchar varchar,
		p_test_smallint smallint,
		p_test_boolean boolean,
		p_test_timestamp timestamp,
		p_test_integer integer,
		p_test_bigint bigint,
		p_test_list_item_fk integer,
		p_test_numeric numeric,
		p_test_date date,
		p_test_complete_id varchar
)
RETURNS integer AS
$body$
DECLARE
    identity int;

BEGIN

    INSERT
	INTO test_completes
	(
		  test_varchar,
		  test_smallint,
		  test_boolean,
		  test_timestamp,
		  test_integer,
		  test_bigint,
		  test_list_item_fk,
		  test_numeric,
		  test_date,
		  test_complete_id
	)
	VALUES
	(
		  p_test_varchar,
		  p_test_smallint,
		  p_test_boolean,
		  p_test_timestamp,
		  p_test_integer,
		  p_test_bigint,
		  p_test_list_item_fk,
		  p_test_numeric,
		  p_test_date,
		  p_test_complete_id
	)

    RETURNING  test_complete_pk INTO identity;
    RETURN identity;

END;
$body$
LANGUAGE 'plpgsql'
VOLATILE
CALLED ON NULL INPUT
SECURITY INVOKER
COST 100;

	

CREATE OR REPLACE FUNCTION public.test_completes_update_by_pk (
        p_test_complete_pk bigint,
		p_test_varchar varchar,
		p_test_smallint smallint,
		p_test_boolean boolean,
		p_test_timestamp timestamp,
		p_test_integer integer,
		p_test_bigint bigint,
		p_test_list_item_fk integer,
		p_test_numeric numeric,
		p_test_date date,
		p_test_complete_id varchar
)
RETURNS integer AS
$body$
DECLARE
  rows_affected int;
  
BEGIN 
  
    UPDATE test_completes
	SET
		test_varchar = p_test_varchar,
		test_smallint = p_test_smallint,
		test_boolean = p_test_boolean,
		test_timestamp = p_test_timestamp,
		test_integer = p_test_integer,
		test_bigint = p_test_bigint,
		test_list_item_fk = p_test_list_item_fk,
		test_numeric = p_test_numeric,
		test_date = p_test_date,
		test_complete_id = p_test_complete_id
	WHERE
		test_complete_pk = p_test_complete_pk;
  
  GET DIAGNOSTICS rows_affected = ROW_COUNT;
  RETURN rows_affected;
   
  EXCEPTION WHEN unique_violation THEN
  RETURN 0;
  
END;
$body$
LANGUAGE 'plpgsql'
VOLATILE
CALLED ON NULL INPUT
SECURITY INVOKER
COST 100;	

	
CREATE OR REPLACE FUNCTION public.test_completes_delete_by_pk (
  p_test_complete_pk bigint
)
RETURNS integer AS
$body$
DECLARE
  rows_affected int;
BEGIN
  
    DELETE
	FROM test_completes
	WHERE
		test_complete_pk = p_test_complete_pk;
  
  GET DIAGNOSTICS rows_affected = ROW_COUNT; 
  RETURN rows_affected; 
   
END;
$body$
LANGUAGE 'plpgsql'
VOLATILE
CALLED ON NULL INPUT
SECURITY INVOKER
COST 100;	


CREATE OR REPLACE FUNCTION public.test_completes_get_by_pk (
   p_test_complete_pk bigint
)
RETURNS TABLE (
        test_complete_pk bigint,
		test_varchar varchar,
		test_smallint smallint,
		test_boolean boolean,
		test_timestamp timestamp,
		test_integer integer,
		test_bigint bigint,
		test_list_item_fk integer,
		test_numeric numeric,
		test_date date,
		test_complete_id varchar
) AS
$body$
BEGIN  
    
	    RETURN QUERY 
	    SELECT  
		c.test_complete_pk::bigint,
		c.test_varchar::varchar,
		c.test_smallint::smallint,
		c.test_boolean::boolean,
		c.test_timestamp::timestamp,
		c.test_integer::integer,
		c.test_bigint::bigint,
		c.test_list_item_fk::integer,
		c.test_numeric::numeric,
		c.test_date::date,
		p_test_complete_id::varchar
	    FROM  test_completes c
	    WHERE test_complete_pk = p_test_complete_pk;
		
   
END;
$body$
LANGUAGE 'plpgsql'
VOLATILE
CALLED ON NULL INPUT
SECURITY INVOKER
COST 100 ROWS 1000;


CREATE OR REPLACE FUNCTION public.test_completes_get_by_id(
  p_test_complete_id varchar, 
  p_match_option varchar
)
RETURNS TABLE (
       	test_complete_pk bigint,
		test_varchar varchar,
		test_smallint smallint,
		test_boolean boolean,
		test_timestamp timestamp,
		test_integer integer,
		test_bigint bigint,
		test_list_item_fk integer,
		test_numeric numeric,
		test_date date,
		test_complete_id varchar
				
) AS
$body$

BEGIN  

    IF (LOWER(p_match_option)='exact') THEN
	  RETURN QUERY 
      SELECT  
	  	c.test_complete_pk::bigint,
		c.test_varchar::varchar,
		c.test_smallint::smallint,
		c.test_boolean::boolean,
		c.test_timestamp::timestamp,
		c.test_integer::integer,
		c.test_bigint::bigint,
		c.test_list_item_fk::integer,
		c.test_numeric::numeric,
		c.test_date::date,
		c.test_complete_id::varchar
       FROM  test_completes c
       WHERE LOWER(c.test_complete_id)=lower(p_test_complete_id);
	ELSE
	  RETURN QUERY 
      SELECT  
	  	c.test_complete_pk::bigint,
		c.test_varchar::varchar,
		c.test_smallint::smallint,
		c.test_boolean::boolean,
		c.test_timestamp::timestamp,
		c.test_integer::integer,
		c.test_bigint::bigint,
		c.test_list_item_fk::integer,
		c.test_numeric::numeric,
		c.test_date::date,
		c.test_complete_id::varchar
	   FROM  test_completes c
       WHERE c.test_complete_id ILIKE '%'||p_test_complete_id||'%';
	END IF;  

END;
$body$
LANGUAGE 'plpgsql'
VOLATILE
CALLED ON NULL INPUT
SECURITY INVOKER




CREATE OR REPLACE FUNCTION public.test_completes_get_by_date (
  p_start_date varchar,
  p_end_date varchar
)
RETURNS TABLE (
    	test_complete_pk bigint,
		test_varchar varchar,
		test_smallint smallint,
		test_boolean boolean,
		test_timestamp timestamp,
		test_integer integer,
		test_bigint bigint,
		test_list_item_fk integer,
		test_numeric numeric,
		test_date date,
		test_complete_id varchar
)
AS
$body$
BEGIN  

      RETURN QUERY 
      SELECT 
	  		c.test_complete_pk::bigint,
			c.test_varchar::varchar,
			c.test_smallint::smallint,
			c.test_boolean::boolean,
			c.test_timestamp::timestamp,
			c.test_integer::integer,
			c.test_bigint::bigint,
			c.test_list_item_fk::integer,
			c.test_numeric::numeric,
			c.test_date::date,
			c.test_complete_id::varchar
	   FROM  test_completes c
       WHERE c.test_timestamp::date 
       BETWEEN p_start_date::date 
       AND p_end_date::date;
	

END;
$body$
LANGUAGE 'plpgsql'
VOLATILE
CALLED ON NULL INPUT
SECURITY INVOKER
COST 100 ROWS 1000;



CREATE OR REPLACE FUNCTION public.test_completes_get_all(
  p_order_by varchar
)
RETURNS SETOF public.test_completes AS
$body$
BEGIN

      RETURN QUERY EXECUTE '
      SELECT *
      FROM   test_completes
      ORDER  BY ' || quote_ident(p_order_by) || '; ';

END;
$body$
LANGUAGE 'plpgsql'
VOLATILE
CALLED ON NULL INPUT
SECURITY INVOKER
COST 100 ROWS 1000;


*/