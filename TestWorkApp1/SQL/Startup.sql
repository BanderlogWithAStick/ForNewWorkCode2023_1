
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS public."Products"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "Barcode" text COLLATE pg_catalog."default" NOT NULL,
    "Title" text COLLATE pg_catalog."default" NOT NULL,
    "Price" money NOT NULL DEFAULT 100.00,
    CONSTRAINT "Products_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Products"
    OWNER to postgres;

COMMENT ON TABLE public."Products"
    IS 'Таблица товаров';


CREATE TABLE IF NOT EXISTS public."Shops"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "Title" text COLLATE pg_catalog."default" NOT NULL,
    "Address" text COLLATE pg_catalog."default",
    CONSTRAINT "Shops_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Shops"
    OWNER to postgres;

COMMENT ON TABLE public."Shops"
    IS 'Таблица магазинов';

CREATE TABLE IF NOT EXISTS public."Sales"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "ShopId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "ProductCount" bigint NOT NULL DEFAULT 1,
    "SaleMoment" timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "Sales_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Sales_ProductId_Products" FOREIGN KEY ("ProductId")
        REFERENCES public."Products" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT "FK_Sales_ShopId_Shops" FOREIGN KEY ("ShopId")
        REFERENCES public."Shops" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Sales"
    OWNER to postgres;

COMMENT ON TABLE public."Sales"
    IS 'Таблица продаж';

CREATE OR REPLACE FUNCTION public.Rep_AVGSales (
	start_period date,
	end_period date,
	shop_ids uuid[],
	product_ids uuid[]
)
RETURNS TABLE(
	"ShopId" uuid,
	"ShopTitle" text,
	"ProductId" uuid,
	"ProductBarcode" text,
	"ProductPrice" money,
	"ProductTitle" text,
	"SumProductCount" integer,
	"RealDaysCount" integer,
	"SaleDay" date
) AS
$$
DECLARE
  	sh_id uuid; -- Переменная для хранения uuid магазина
  	pr_id uuid; -- Переменная для хранения uuid продукта
	real_days_count integer; -- Для вычисления реального числа дней для товара
BEGIN
	CREATE TEMPORARY TABLE IF NOT EXISTS "avg_table"(
		"ShopId" uuid,
		"ShopTitle" text,
		"ProductId" uuid,
		"ProductBarcode" text,
		"ProductPrice" money,
		"ProductTitle" text,
		"SumProductCount" integer,
		"RealDaysCount" integer,
		"SaleDay" date
	);
	TRUNCATE "avg_table";
	
  	FOREACH sh_id IN ARRAY shop_ids
  	LOOP
    	RAISE NOTICE 'id = %,', sh_id;
		
		FOREACH pr_id IN ARRAY product_ids
		LOOP
			
			SELECT
			COUNT(DISTINCT(DATE("S"."SaleMoment")))
			INTO real_days_count
			FROM public."Sales" AS "S"
			WHERE 
			"S"."ShopId" = sh_id AND
			"S"."ProductId" = pr_id AND
			DATE("S"."SaleMoment") BETWEEN start_period AND end_period;
			
			INSERT INTO "avg_table" ("ShopId", "ShopTitle", "ProductId", "ProductBarcode", "ProductPrice", "ProductTitle", "SumProductCount", "RealDaysCount", "SaleDay")
			SELECT
				"Sh"."Id",
				"Sh"."Title",
				"P"."Id",
				"P"."Barcode",
				"P"."Price",
				"P"."Title",
				SUM("ProductCount"),
				real_days_count,
				DATE("S"."SaleMoment")
			FROM public."Sales" AS "S"
			INNER JOIN public."Shops" AS "Sh" ON "Sh"."Id" = "S"."ShopId"
			INNER JOIN public."Products" AS "P" ON "P"."Id" = "S"."ProductId"
			WHERE
			"S"."ShopId" = sh_id AND
			"S"."ProductId" = pr_id AND
			DATE("S"."SaleMoment") BETWEEN start_period AND end_period
			GROUP BY "Sh"."Id", DATE("S"."SaleMoment"), "P"."Id";
			
		END LOOP;
		
	END LOOP;
    RETURN QUERY SELECT * FROM "avg_table";
END;
$$ LANGUAGE plpgsql;