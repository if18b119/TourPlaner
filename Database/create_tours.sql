CREATE TABLE IF NOT EXISTS public.tours
(
    tour_id character varying COLLATE pg_catalog."default" NOT NULL,
    name character varying COLLATE pg_catalog."default" NOT NULL,
    description character varying COLLATE pg_catalog."default",
    start character varying COLLATE pg_catalog."default" NOT NULL,
    destination character varying COLLATE pg_catalog."default" NOT NULL,
    route_type character varying COLLATE pg_catalog."default" NOT NULL,
    path character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT tours_pkey PRIMARY KEY (tour_id)
)

TABLESPACE pg_default;

ALTER TABLE public.tours
    OWNER to postgres;