CREATE TABLE exchange_rates(
	exchange_rate_id bigserial primary key,
	date date not null,
	source_currency text not null,
	target_currency text not null,
	value float
);

CREATE TABLE api_keys(
	api_key uuid primary key
);

CREATE UNIQUE INDEX CONCURRENTLY exchange_rates_uq
ON exchange_rates (date, source_currency, target_currency);