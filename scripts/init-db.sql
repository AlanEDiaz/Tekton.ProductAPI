USE TektonProduct;
CREATE USER sa WITH PASSWORD = 'Pass@word123';
ALTER ROLE db_owner ADD MEMBER sa;