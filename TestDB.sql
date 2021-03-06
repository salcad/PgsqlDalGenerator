
-- Database  : postgres
-- Version     : PostgreSQL 9.0.4,


SET search_path = public, pg_catalog;
DROP INDEX IF EXISTS public.web_users_idx;
DROP INDEX IF EXISTS public.web_roles_idx;
DROP INDEX IF EXISTS public.tests_idx;
DROP FUNCTION IF EXISTS public.test_completes_get_by_pk (p_test_complete_pk bigint);
DROP FUNCTION IF EXISTS public.test_completes_get_by_date (p_start_date varchar, p_end_date varchar);
DROP FUNCTION IF EXISTS public.test_completes_get_by_id (p_test_complete_id varchar, p_match_option varchar);
DROP FUNCTION IF EXISTS public.test_completes_delete_by_pk (p_test_complete_pk bigint);
DROP FUNCTION IF EXISTS public.test_completes_update_by_pk (p_test_complete_pk bigint, p_test_varchar varchar, p_test_smallint smallint, p_test_boolean boolean, p_test_timestamp timestamp without time zone, p_test_integer integer, p_test_bigint bigint, p_test_list_item_fk integer, p_test_numeric numeric, p_test_date date, p_test_complete_id varchar);
DROP FUNCTION IF EXISTS public.test_completes_insert (p_test_varchar varchar, p_test_smallint smallint, p_test_boolean boolean, p_test_timestamp timestamp without time zone, p_test_integer integer, p_test_bigint bigint, p_test_list_item_fk integer, p_test_numeric numeric, p_test_date date, p_test_complete_id varchar);
DROP FUNCTION IF EXISTS public.meta_get_qty_unique_index_other_than_pk (p_table_name varchar);
DROP FUNCTION IF EXISTS public.tests_get_by_id (p_test_id varchar, p_match_option varchar);
DROP FUNCTION IF EXISTS public.web_users_get_by_pk (p_web_user_pk integer);
DROP FUNCTION IF EXISTS public.web_users_insert (p_web_user_id varchar, p_password varchar, p_is_enabled boolean, p_password_sum_error integer, p_web_role_fk integer, p_web_user_status_fk smallint);
DROP FUNCTION IF EXISTS public.meta_extract_table_fks (p_table_name varchar);
DROP SEQUENCE IF EXISTS public.test_completes_test_complete_pk_seq;
DROP TABLE IF EXISTS public.test_complete_details;
DROP TABLE IF EXISTS public.test_list_items;
DROP TABLE IF EXISTS public.test_completes;
DROP FUNCTION IF EXISTS public.meta_extract_function_args (p_funcname varchar, p_schema varchar, out pos integer, out direction char, out argname varchar, out datatype varchar);
DROP FUNCTION IF EXISTS public.meta_extract_table_pk (p_table_name varchar);
DROP FUNCTION IF EXISTS public.meta_extract_table_fields (p_table_name varchar);
DROP FUNCTION IF EXISTS public.select_test2 ();
DROP FUNCTION IF EXISTS public.select_test ();
DROP TABLE IF EXISTS public.web_user_status;
DROP TABLE IF EXISTS public.web_roles;
DROP SEQUENCE IF EXISTS public.web_roles_web_role_pk_seq;
DROP TABLE IF EXISTS public.web_users;
DROP SEQUENCE IF EXISTS public.web_users_web_user_pk_seq;
DROP SEQUENCE IF EXISTS public.tests_test_pk_seq;
DROP TABLE IF EXISTS public.test_details;
DROP TABLE IF EXISTS public.tests;
DROP FUNCTION IF EXISTS public.execute_dynamic_select (query_string text);
DROP FUNCTION IF EXISTS public.web_users_change_password (web_user_id varchar, new_password varchar, old_password varchar);
DROP FUNCTION IF EXISTS public.tests_insert (p_test_id varchar);
DROP FUNCTION IF EXISTS public.tests_update_by_pk (p_test_pk integer, p_test_id varchar);
DROP FUNCTION IF EXISTS public.tests_delete_by_pk (p_test_pk integer);
DROP FUNCTION IF EXISTS public.tests_get_qty_by_ilike (p_test_id varchar);
DROP FUNCTION IF EXISTS public.tests_get_by_pk (p_test_pk integer);
DROP FUNCTION IF EXISTS public.tests_get_all ();
--
-- Definition for function tests_get_all (OID = 16384) : 
--
SET check_function_bodies = false;
CREATE FUNCTION public.tests_get_all (
)
RETURNS refcursor
AS 
$body$ 
DECLARE oCursor REFCURSOR; 
BEGIN 
OPEN oCursor 
FOR SELECT * 
FROM public.Tests; 
RETURN oCursor; 
END; 
$body$
    LANGUAGE plpgsql;
--
-- Definition for function tests_get_by_pk (OID = 16387) : 
--
CREATE FUNCTION public.tests_get_by_pk (
  p_test_pk integer
)
RETURNS refcursor
AS 
$body$
DECLARE 
    oCursor REFCURSOR; 
BEGIN  
    OPEN oCursor 
    FOR SELECT * 
    FROM public.tests t
    WHERE t.test_pk=p_test_pk; 
    RETURN oCursor;  
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function tests_get_qty_by_ilike (OID = 16388) : 
--
CREATE FUNCTION public.tests_get_qty_by_ilike (
  p_test_id character varying
)
RETURNS integer
AS 
$body$
DECLARE
 qty int;
BEGIN
 SELECT COUNT(test_pk) INTO qty
 FROM tests t
 WHERE t.test_id ILIKE '%'||p_test_id||'%';
 RETURN qty;
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function tests_delete_by_pk (OID = 16389) : 
--
CREATE FUNCTION public.tests_delete_by_pk (
  p_test_pk integer
)
RETURNS integer
AS 
$body$
DECLARE
  rows_affected int;
BEGIN
  
  DELETE FROM tests
  WHERE test_pk=p_test_pk;  
  
  GET DIAGNOSTICS rows_affected = ROW_COUNT; 
  
  RETURN rows_affected; 
   
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function tests_update_by_pk (OID = 16390) : 
--
CREATE FUNCTION public.tests_update_by_pk (
  p_test_pk integer,
  p_test_id character varying
)
RETURNS integer
AS 
$body$
DECLARE
  rows_affected int;
  
BEGIN 
  
  UPDATE public.tests 
  SET 
    test_id = p_test_id
  WHERE test_pk = p_test_pk; 
  
  GET DIAGNOSTICS rows_affected = ROW_COUNT;
  RETURN rows_affected;
   
  EXCEPTION WHEN unique_violation THEN
  RETURN 0;
  
   
  
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function tests_insert (OID = 16391) : 
--
CREATE FUNCTION public.tests_insert (
  p_test_id character varying
)
RETURNS integer
AS 
$body$
DECLARE
identity int;

BEGIN

INSERT INTO tests
(
  test_id
) 
VALUES
(
  p_test_id
) 

RETURNING test_pk INTO identity;
RETURN identity;

END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function web_users_change_password (OID = 16393) : 
--
CREATE FUNCTION public.web_users_change_password (
  web_user_id character varying,
  new_password character varying,
  old_password character varying
)
RETURNS varchar
AS 
$body$
DECLARE
   web_role_fk  INTEGER;
   web_role_fk_member  INTEGER;
   result varchar;
BEGIN
   result := 'ok';
         
   IF NOT EXISTS(
   SELECT *
   FROM web_users wu
   WHERE lower(wu.web_user_id) = lower(web_user_id) AND
   wu.password = old_password)
   THEN
      result := 'invalid_password';
      RETURN result;
   END IF;
  
   UPDATE web_users wu
   SET password = new_password
   WHERE wu.web_user_id = web_user_id;
   
   RETURN result;
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function execute_dynamic_select (OID = 16395) : 
--
CREATE FUNCTION public.execute_dynamic_select (
  query_string text
)
RETURNS refcursor
AS 
$body$
DECLARE 
  oCursor REFCURSOR;
  
BEGIN 

    IF (SELECT COUNT("col") 
        FROM unnest(string_to_array(lower(query_string),' '))  d("col") 
        WHERE col='update' 
        OR    col='delete' 
        OR    col='insert'
        OR    col='alter'
        OR    col='create' 
        OR    col='index'
        OR    col='drop') > 0
    THEN
        RETURN oCursor;
    END IF;  
    
    IF (SELECT COUNT("col") 
        FROM unnest(string_to_array(lower(query_string),' '))  d("col") 
        WHERE col='from') = 0
    THEN
        RETURN oCursor;
    END IF;  

 
  OPEN oCursor 
  FOR EXECUTE lower(query_string); 
  RETURN oCursor;  

END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function select_test (OID = 16449) : 
--
CREATE FUNCTION public.select_test (
)
RETURNS refcursor
AS 
$body$
DECLARE
    cursor1 CURSOR FOR select * from tests;
BEGIN
 open cursor1;
 return (cursor1);
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function select_test2 (OID = 16451) : 
--
CREATE FUNCTION public.select_test2 (
)
RETURNS SETOF tests
AS 
$body$
DECLARE
  rc tests%ROWTYPE;
BEGIN
  FOR rc IN SELECT * FROM tests LOOP
    RETURN NEXT rc;
  END LOOP;
END;$body$
    LANGUAGE plpgsql;
--
-- Definition for function meta_extract_table_fields (OID = 16463) : 
--
CREATE FUNCTION public.meta_extract_table_fields (
  p_table_name character varying
)
RETURNS TABLE (
  ordinal_position integer,
  column_name character varying,
  data_type character varying,
  column_default character varying,
  is_nullable boolean,
  character_maximum_length integer,
  numeric_precision integer
)
AS 
$body$
BEGIN  
    RETURN QUERY 
    SELECT c.ordinal_position::integer,
           c.column_name::varchar,
           c.data_type::varchar,
           c.column_default::varchar,
           c.is_nullable::boolean,
           c.character_maximum_length::integer,
           c.numeric_precision::integer
    FROM information_schema.columns c
    WHERE c.table_name = p_table_name
    ORDER BY c.ordinal_position; 
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function meta_extract_table_pk (OID = 16464) : 
--
CREATE FUNCTION public.meta_extract_table_pk (
  p_table_name character varying
)
RETURNS varchar
AS 
$body$
DECLARE
       col_name varchar;
BEGIN          
          SELECT
                (select attname 
                 from pg_attribute 
                 where attrelid = pg_constraint.conrelid and 
                 attnum = pg_constraint.conkey[1]) INTO col_name
		  FROM
		 	pg_constraint
		  WHERE
		  pg_constraint.contype = 'p'
          AND
          pg_constraint.conrelid = 
                (SELECT pg_class.oid
                        FROM 
                        pg_class
                        INNER JOIN pg_user ON (pg_class.relowner = pg_user.usesysid)
                        INNER JOIN pg_namespace ON (pg_class.relnamespace = pg_namespace.oid)
                        WHERE
                        pg_class.relkind IN ('r','v')
                        AND pg_namespace.nspname NOT IN ('pg_catalog','information_schema')
                        AND pg_class.relname=p_table_name);
         RETURN col_name;
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function meta_extract_function_args (OID = 16471) : 
--
CREATE FUNCTION public.meta_extract_function_args (
  p_funcname character varying,
  p_schema character varying,
  out pos integer,
  out direction character,
  out argname character varying,
  out datatype character varying
)
RETURNS SETOF record
AS 
$body$
DECLARE
  rettype character varying;
  argtypes oidvector;
  allargtypes oid[];
  argmodes "char"[];
  argnames text[];
  mini integer;
  maxi integer;
BEGIN
  /* get object ID of function */
  SELECT INTO rettype, argtypes, allargtypes, argmodes, argnames
         CASE
         WHEN pg_proc.proretset
         THEN 'setof ' || pg_catalog.format_type(pg_proc.prorettype, NULL)
         ELSE pg_catalog.format_type(pg_proc.prorettype, NULL) END,
         pg_proc.proargtypes,
         pg_proc.proallargtypes,
         pg_proc.proargmodes,
         pg_proc.proargnames
    FROM pg_catalog.pg_proc
         JOIN pg_catalog.pg_namespace
         ON (pg_proc.pronamespace = pg_namespace.oid)
   WHERE pg_proc.prorettype <> 'pg_catalog.cstring'::pg_catalog.regtype
     AND (pg_proc.proargtypes[0] IS NULL
      OR pg_proc.proargtypes[0] <> 'pg_catalog.cstring'::pg_catalog.regtype)
     AND NOT pg_proc.proisagg
     AND pg_proc.proname = p_funcname
     AND pg_namespace.nspname = p_schema
     AND pg_catalog.pg_function_is_visible(pg_proc.oid);
 
  /* bail out if not found */
  IF NOT FOUND THEN
    RETURN;
  END IF;
 
  /* return a row for the return value */
  pos = 0;
  direction = 'o'::char;
  argname = 'RETURN VALUE';
  datatype = rettype;
  RETURN NEXT;
 
  /* unfortunately allargtypes is NULL if there are no OUT parameters */
  IF allargtypes IS NULL THEN
    mini = array_lower(argtypes, 1); maxi = array_upper(argtypes, 1);
  ELSE
    mini = array_lower(allargtypes, 1); maxi = array_upper(allargtypes, 1);
  END IF;
  IF maxi < mini THEN RETURN; END IF;
 
  /* loop all the arguments */
  FOR i IN mini .. maxi LOOP
    pos = i - mini + 1;
    IF argnames IS NULL THEN
      argname = NULL;
    ELSE
      argname = argnames[pos];
    END IF;
    IF allargtypes IS NULL THEN
      direction = 'i'::char;
      datatype = pg_catalog.format_type(argtypes[i], NULL);
    ELSE
      direction = argmodes[i];
      datatype = pg_catalog.format_type(allargtypes[i], NULL);
    END IF;
    RETURN NEXT;
  END LOOP;
 
  RETURN;
END;
$body$
    LANGUAGE plpgsql STABLE STRICT;
--
-- Definition for function meta_extract_table_fks (OID = 16563) : 
--
CREATE FUNCTION public.meta_extract_table_fks (
  p_table_name character varying
)
RETURNS TABLE (
  ref_table character varying,
  foreign_col character varying,
  ref_col character varying
)
AS 
$body$
BEGIN  
          RETURN QUERY 
          SELECT
                (SELECT 
				pg_class.relname
			    FROM 
				pg_class
				INNER JOIN pg_user ON (pg_class.relowner = pg_user.usesysid)
				INNER JOIN pg_namespace ON (pg_class.relnamespace = pg_namespace.oid)
			    WHERE
				pg_class.relkind IN ('r','v')
				AND pg_namespace.nspname NOT IN ('pg_catalog','information_schema')
                AND pg_class.oid=pg_constraint.confrelid )::varchar as ref_table,     
                (select attname 
                 from pg_attribute 
                 where attrelid = pg_constraint.conrelid  and 
                 attnum = pg_constraint.conkey[01])::varchar as foreign_col,
				(select attname 
                from pg_attribute 
                where attrelid = pg_constraint.confrelid and 
                attnum = pg_constraint.confkey[01])::varchar as ref_col
			FROM
				pg_constraint          
			WHERE
				pg_constraint.contype = 'f' AND
                pg_constraint.conrelid = 
                 (SELECT pg_class.oid
                        FROM 
                        pg_class
                        INNER JOIN pg_user ON (pg_class.relowner = pg_user.usesysid)
                        INNER JOIN pg_namespace ON (pg_class.relnamespace = pg_namespace.oid)
                        WHERE
                        pg_class.relkind IN ('r','v')
                        AND pg_namespace.nspname NOT IN ('pg_catalog','information_schema')
                        AND pg_class.relname=p_table_name);
                        
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function web_users_insert (OID = 16564) : 
--
CREATE FUNCTION public.web_users_insert (
  p_web_user_id character varying,
  p_password character varying,
  p_is_enabled boolean,
  p_password_sum_error integer,
  p_web_role_fk integer,
  p_web_user_status_fk smallint
)
RETURNS integer
AS 
$body$
DECLARE
    identity int;

BEGIN

    INSERT
	INTO web_users
	(
		  web_user_id,
		  password,
		  is_enabled,
		  password_sum_error,
		  web_role_fk,
		  web_user_status_fk
	)
	VALUES
	(
		  p_web_user_id,
		  p_password,
		  p_is_enabled,
		  p_password_sum_error,
		  p_web_role_fk,
		  p_web_user_status_fk
	)

    RETURNING  web_user_pk INTO identity;
    RETURN identity;

END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function web_users_get_by_pk (OID = 16565) : 
--
CREATE FUNCTION public.web_users_get_by_pk (
  p_web_user_pk integer
)
RETURNS TABLE (
  web_user_pk integer,
  web_user_id character varying,
  password character varying,
  is_enabled boolean,
  password_sum_error integer,
  web_role_fk integer,
  web_user_status_fk smallint
)
AS 
$body$
BEGIN  
    
	RETURN QUERY 
    SELECT  
		c.web_user_pk::integer,
		c.web_user_id::varchar,
		c.password::varchar,
		c.is_enabled::boolean,
		c.password_sum_error::integer,
		c.web_role_fk::integer,
		p_web_user_status_fk::smallint
    FROM  web_users c
    WHERE web_user_pk = p_web_user_pk;
   
END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function tests_get_by_id (OID = 16566) : 
--
CREATE FUNCTION public.tests_get_by_id (
  p_test_id character varying,
  p_match_option character varying
)
RETURNS TABLE (
  test_pk integer,
  test_id character varying
)
AS 
$body$
BEGIN  

    IF (rtrim(lower(p_match_option))='exact') THEN
	  RETURN QUERY 
      SELECT  
		c.test_pk::integer,
		c.test_id::varchar
      FROM  tests c
      WHERE lower(c.test_id)=lower(p_test_id);
	ELSE
	  RETURN QUERY 
      SELECT  
		c.test_pk::integer,
		c.test_id::varchar
      FROM  tests c
      WHERE c.test_id ILIKE '%'||p_test_id||'%';
	END IF;  

END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function meta_get_qty_unique_index_other_than_pk (OID = 16568) : 
--
CREATE FUNCTION public.meta_get_qty_unique_index_other_than_pk (
  p_table_name character varying
)
RETURNS integer
AS 
$body$
DECLARE
 qty int;
BEGIN

 SELECT COUNT(c.relname) INTO qty
 FROM pg_index a, 
      pg_class b, 
      pg_class c 
 WHERE b.relname=p_table_name 
 AND b.oid=a.indrelid 
 AND a.indexrelid=c.oid 
 AND a.indisprimary=false
 AND a.indisunique=true;
 
 RETURN qty;

 END;
$body$
    LANGUAGE plpgsql;
--
-- Definition for function test_completes_insert (OID = 16582) : 
--
CREATE FUNCTION public.test_completes_insert (
  p_test_varchar character varying,
  p_test_smallint smallint,
  p_test_boolean boolean,
  p_test_timestamp timestamp without time zone,
  p_test_integer integer,
  p_test_bigint bigint,
  p_test_list_item_fk integer,
  p_test_numeric numeric,
  p_test_date date,
  p_test_complete_id character varying
)
RETURNS integer
AS 
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
    LANGUAGE plpgsql;
--
-- Definition for function test_completes_update_by_pk (OID = 16584) : 
--
CREATE FUNCTION public.test_completes_update_by_pk (
  p_test_complete_pk bigint,
  p_test_varchar character varying,
  p_test_smallint smallint,
  p_test_boolean boolean,
  p_test_timestamp timestamp without time zone,
  p_test_integer integer,
  p_test_bigint bigint,
  p_test_list_item_fk integer,
  p_test_numeric numeric,
  p_test_date date,
  p_test_complete_id character varying
)
RETURNS integer
AS 
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
    LANGUAGE plpgsql;
--
-- Definition for function test_completes_delete_by_pk (OID = 16585) : 
--
CREATE FUNCTION public.test_completes_delete_by_pk (
  p_test_complete_pk bigint
)
RETURNS integer
AS 
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
    LANGUAGE plpgsql;
--
-- Definition for function test_completes_get_by_id (OID = 16587) : 
--
CREATE FUNCTION public.test_completes_get_by_id (
  p_test_complete_id character varying,
  p_match_option character varying
)
RETURNS TABLE (
  test_complete_pk bigint,
  test_varchar character varying,
  test_smallint smallint,
  test_boolean boolean,
  test_timestamp timestamp without time zone,
  test_integer integer,
  test_bigint bigint,
  test_list_item_fk integer,
  test_numeric numeric,
  test_date date,
  test_complete_id character varying
)
AS 
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
    LANGUAGE plpgsql;
--
-- Definition for function test_completes_get_by_date (OID = 16589) : 
--
CREATE FUNCTION public.test_completes_get_by_date (
  p_start_date character varying,
  p_end_date character varying
)
RETURNS TABLE (
  test_complete_pk bigint,
  test_varchar character varying,
  test_smallint smallint,
  test_boolean boolean,
  test_timestamp timestamp without time zone,
  test_integer integer,
  test_bigint bigint,
  test_list_item_fk integer,
  test_numeric numeric,
  test_date date,
  test_complete_id character varying
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
    LANGUAGE plpgsql;
--
-- Definition for function test_completes_get_by_pk (OID = 16590) : 
--
CREATE FUNCTION public.test_completes_get_by_pk (
  p_test_complete_pk bigint
)
RETURNS TABLE (
  test_complete_pk bigint,
  test_varchar character varying,
  test_smallint smallint,
  test_boolean boolean,
  test_timestamp timestamp without time zone,
  test_integer integer,
  test_bigint bigint,
  test_list_item_fk integer,
  test_numeric numeric,
  test_date date,
  test_complete_id character varying
)
AS 
$body$
BEGIN  
    
    IF (p_test_complete_pk=-9) THEN
    
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
      FROM  test_completes c;   
         
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
          p_test_complete_id::varchar
      FROM  test_completes c
      WHERE test_complete_pk = p_test_complete_pk;
    
    END IF;
   
END;
$body$
    LANGUAGE plpgsql;
--
-- Structure for table tests (OID = 16396) : 
--
CREATE TABLE public.tests (
    test_pk integer DEFAULT nextval(('public.tests_test_pk_seq'::text)::regclass) NOT NULL,
    test_id varchar(100) NOT NULL
) WITHOUT OIDS;
--
-- Structure for table test_details (OID = 16400) : 
--
CREATE TABLE public.test_details (
    test_detail_pk integer NOT NULL,
    test_detail_content varchar(100),
    test_fk integer NOT NULL
) WITHOUT OIDS;
--
-- Definition for sequence tests_test_pk_seq (OID = 16403) : 
--
CREATE SEQUENCE public.tests_test_pk_seq
    START WITH 1
    INCREMENT BY 1
    MAXVALUE 2147483647
    NO MINVALUE
    CACHE 1;
--
-- Definition for sequence web_users_web_user_pk_seq (OID = 16405) : 
--
CREATE SEQUENCE public.web_users_web_user_pk_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;
--
-- Structure for table web_users (OID = 16407) : 
--
CREATE TABLE public.web_users (
    web_user_pk integer DEFAULT nextval('web_users_web_user_pk_seq'::regclass) NOT NULL,
    web_user_id varchar(30) NOT NULL,
    password varchar(250),
    is_enabled boolean DEFAULT true NOT NULL,
    password_sum_error integer DEFAULT 0 NOT NULL,
    web_role_fk integer NOT NULL,
    web_user_status_fk smallint NOT NULL
) WITHOUT OIDS;
--
-- Definition for sequence web_roles_web_role_pk_seq (OID = 16413) : 
--
CREATE SEQUENCE public.web_roles_web_role_pk_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;
--
-- Structure for table web_roles (OID = 16415) : 
--
CREATE TABLE public.web_roles (
    web_role_pk integer DEFAULT nextval('web_roles_web_role_pk_seq'::regclass) NOT NULL,
    web_role_id varchar(50) NOT NULL,
    description varchar(300)
) WITHOUT OIDS;
--
-- Structure for table web_user_status (OID = 16419) : 
--
CREATE TABLE public.web_user_status (
    web_user_status_pk smallint NOT NULL,
    web_user_status_id varchar(50) NOT NULL
) WITHOUT OIDS;
--
-- Structure for table test_completes (OID = 16472) : 
--
CREATE TABLE public.test_completes (
    test_complete_pk bigint DEFAULT nextval(('public.test_completes_test_complete_pk_seq'::text)::regclass) NOT NULL,
    test_varchar varchar(100),
    test_smallint smallint DEFAULT 0 NOT NULL,
    test_boolean boolean DEFAULT false NOT NULL,
    test_timestamp timestamp(0) without time zone DEFAULT now() NOT NULL,
    test_integer integer DEFAULT 0 NOT NULL,
    test_bigint bigint DEFAULT 0 NOT NULL,
    test_list_item_fk integer,
    test_numeric numeric(19,4) DEFAULT 0.0 NOT NULL,
    test_date date DEFAULT ('now'::text)::date NOT NULL,
    test_complete_id varchar(100) NOT NULL
) WITHOUT OIDS;
--
-- Structure for table test_list_items (OID = 16485) : 
--
CREATE TABLE public.test_list_items (
    test_list_item_pk integer NOT NULL,
    test_list_item_id varchar(100) NOT NULL
) WITHOUT OIDS;
--
-- Structure for table test_complete_details (OID = 16492) : 
--
CREATE TABLE public.test_complete_details (
    test_detail_pk integer NOT NULL,
    test_detail_id varchar(100) NOT NULL,
    test_complete_fk bigint
) WITHOUT OIDS;
--
-- Definition for sequence test_completes_test_complete_pk_seq (OID = 16517) : 
--
CREATE SEQUENCE public.test_completes_test_complete_pk_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;
--
-- Data for table public.tests (OID = 16396) (LIMIT 0,42)
--
INSERT INTO tests (test_pk, test_id)
VALUES (8, 'Test1');

INSERT INTO tests (test_pk, test_id)
VALUES (2, 'Ok1');

INSERT INTO tests (test_pk, test_id)
VALUES (10, 'Test 9/16/2011 4:17:54 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (11, 'Test 9/16/2011 4:18:09 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (12, 'Test 9/16/2011 4:18:45 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (13, 'Test 11:19:15 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (14, 'Test 11:19:39 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (15, 'Test 11:22:59 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (16, 'Test 11:23:24 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (17, 'Test 11:24:40 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (18, 'Test 11:26:23 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (19, 'Test 12:41:57 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (20, 'Test 12:48:12 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (23, 'Test 12:57:28 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (24, 'Test 1:19:58 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (25, 'Test 1:21:45 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (26, 'Test 1:23:57 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (27, 'Test 1:24:47 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (28, 'Test 1:25:03 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (29, 'Test 1:27:13 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (30, 'Test 1:27:28 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (31, 'Test 1:28:27 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (32, 'Test 1:28:43 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (33, 'Test 1:29:56 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (34, 'Test 1:32:04 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (3, 'Happy');

INSERT INTO tests (test_pk, test_id)
VALUES (35, 'Test 10:51:56 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (36, 'Test 10:52:39 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (37, 'Helo3:00:08 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (38, 'Helo3:00:30 PM');

INSERT INTO tests (test_pk, test_id)
VALUES (41, 'Helo9/26/2011 9:07:54 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (42, 'Helo9/26/2011 9:08:19 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (43, 'Helo9/26/2011 9:08:27 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (44, 'Test:11:10:10 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (45, 'Test:11:11:57 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (4, 'Testis');

INSERT INTO tests (test_pk, test_id)
VALUES (46, 'Test:11:14:07 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (47, 'Test:11:16:13 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (48, 'Test:11:32:09 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (49, 'Test:11:36:25 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (50, 'Test:11:37:23 AM');

INSERT INTO tests (test_pk, test_id)
VALUES (51, 'Test:11:38:55 AM');

--
-- Definition for index tests_idx (OID = 16422) : 
--
CREATE UNIQUE INDEX tests_idx ON tests USING btree (lower((test_id)::text));
--
-- Definition for index web_roles_idx (OID = 16423) : 
--
CREATE UNIQUE INDEX web_roles_idx ON web_roles USING btree (lower((web_role_id)::text));
--
-- Definition for index web_users_idx (OID = 16571) : 
--
CREATE UNIQUE INDEX web_users_idx ON web_users USING btree (((lower((web_user_id)::text))::character varying));
--
-- Definition for index test_details_pkey (OID = 16424) : 
--
ALTER TABLE ONLY test_details
    ADD CONSTRAINT test_details_pkey
    PRIMARY KEY (test_detail_pk);
--
-- Definition for index tests_pkey (OID = 16426) : 
--
ALTER TABLE ONLY tests
    ADD CONSTRAINT tests_pkey
    PRIMARY KEY (test_pk);
--
-- Definition for index test_details_fk (OID = 16428) : 
--
ALTER TABLE ONLY test_details
    ADD CONSTRAINT test_details_fk
    FOREIGN KEY (test_fk) REFERENCES tests(test_pk) ON UPDATE RESTRICT ON DELETE RESTRICT;
--
-- Definition for index web_roles_pkey (OID = 16433) : 
--
ALTER TABLE ONLY web_roles
    ADD CONSTRAINT web_roles_pkey
    PRIMARY KEY (web_role_pk);
--
-- Definition for index web_users_fk (OID = 16435) : 
--
ALTER TABLE ONLY web_users
    ADD CONSTRAINT web_users_fk
    FOREIGN KEY (web_role_fk) REFERENCES web_roles(web_role_pk) ON UPDATE RESTRICT ON DELETE RESTRICT;
--
-- Definition for index web_user_status_pkey (OID = 16440) : 
--
ALTER TABLE ONLY web_user_status
    ADD CONSTRAINT web_user_status_pkey
    PRIMARY KEY (web_user_status_pk);
--
-- Definition for index web_users_fk1 (OID = 16442) : 
--
ALTER TABLE ONLY web_users
    ADD CONSTRAINT web_users_fk1
    FOREIGN KEY (web_user_status_fk) REFERENCES web_user_status(web_user_status_pk) ON UPDATE RESTRICT ON DELETE RESTRICT;
--
-- Definition for index web_users_pkey (OID = 16468) : 
--
ALTER TABLE ONLY web_users
    ADD CONSTRAINT web_users_pkey
    PRIMARY KEY (web_user_pk);
--
-- Definition for index test_list_pkey (OID = 16488) : 
--
ALTER TABLE ONLY test_list_items
    ADD CONSTRAINT test_list_pkey
    PRIMARY KEY (test_list_item_pk);
--
-- Definition for index test_list_items_test_list_item_id_key (OID = 16490) : 
--
ALTER TABLE ONLY test_list_items
    ADD CONSTRAINT test_list_items_test_list_item_id_key
    UNIQUE (test_list_item_id);
--
-- Definition for index test_complete_details_pkey (OID = 16495) : 
--
ALTER TABLE ONLY test_complete_details
    ADD CONSTRAINT test_complete_details_pkey
    PRIMARY KEY (test_detail_pk);
--
-- Definition for index test_complete_pkey (OID = 16519) : 
--
ALTER TABLE ONLY test_completes
    ADD CONSTRAINT test_complete_pkey
    PRIMARY KEY (test_complete_pk);
--
-- Definition for index test_complete_details_fk (OID = 16521) : 
--
ALTER TABLE ONLY test_complete_details
    ADD CONSTRAINT test_complete_details_fk
    FOREIGN KEY (test_complete_fk) REFERENCES test_completes(test_complete_pk) ON UPDATE RESTRICT ON DELETE RESTRICT;
--
-- Definition for index test_completes_fk1 (OID = 16538) : 
--
ALTER TABLE ONLY test_completes
    ADD CONSTRAINT test_completes_fk1
    FOREIGN KEY (test_list_item_fk) REFERENCES test_list_items(test_list_item_pk);
--
-- Definition for index test_completes_test_complete_id_key (OID = 16580) : 
--
ALTER TABLE ONLY test_completes
    ADD CONSTRAINT test_completes_test_complete_id_key
    UNIQUE (test_complete_id);
--
-- Data for sequence public.tests_test_pk_seq (OID = 16403)
--
SELECT pg_catalog.setval('tests_test_pk_seq', 51, true);
--
-- Data for sequence public.web_users_web_user_pk_seq (OID = 16405)
--
SELECT pg_catalog.setval('web_users_web_user_pk_seq', 1, false);
--
-- Data for sequence public.web_roles_web_role_pk_seq (OID = 16413)
--
SELECT pg_catalog.setval('web_roles_web_role_pk_seq', 1, false);
--
-- Data for sequence public.test_completes_test_complete_pk_seq (OID = 16517)
--
SELECT pg_catalog.setval('test_completes_test_complete_pk_seq', 999, true);
--
-- Comments
--
COMMENT ON SCHEMA public IS 'standard public schema';
