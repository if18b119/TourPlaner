CREATE TABLE IF NOT EXISTS public.logs
(
    log_id character varying COLLATE pg_catalog."default" NOT NULL,
    fk_tour_id character varying COLLATE pg_catalog."default" NOT NULL,
    date_time character varying COLLATE pg_catalog."default",
    distance character varying COLLATE pg_catalog."default",
    total_time character varying COLLATE pg_catalog."default",
    report character varying COLLATE pg_catalog."default",
    rating character varying COLLATE pg_catalog."default",
    avarage_speed character varying COLLATE pg_catalog."default",
    comment character varying COLLATE pg_catalog."default",
    problems character varying COLLATE pg_catalog."default",
    transport_modus character varying COLLATE pg_catalog."default",
    recomended character varying COLLATE pg_catalog."default",
    CONSTRAINT logs_pkey PRIMARY KEY (log_id),
    CONSTRAINT fk_tour_id FOREIGN KEY (fk_tour_id)
        REFERENCES public.tours (tour_id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE public.logs
    OWNER to postgres;
-- Index: fki_fk_tour_id

-- DROP INDEX public.fki_fk_tour_id;

CREATE INDEX fki_fk_tour_id
    ON public.logs USING btree
    (fk_tour_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;