CREATE TABLE IF NOT EXISTS public.tourinfos
(
    tour_id character varying COLLATE pg_catalog."default" NOT NULL,
    distance character varying COLLATE pg_catalog."default",
    "time" character varying COLLATE pg_catalog."default",
    has_tunnels boolean,
    has_highways boolean,
    has_bridge boolean,
    has_acces_restriction boolean,
    CONSTRAINT tourinfos_pkey PRIMARY KEY (tour_id),
    CONSTRAINT fk_tour_id FOREIGN KEY (tour_id)
        REFERENCES public.tours (tour_id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE public.tourinfos
    OWNER to postgres;