.PHONY: up down pull build config

include docker-compose.mk

up:
	docker compose $(DOTNEST_COMPOSE_ARGS) up --build -d --remove-orphans

down:
	docker compose $(DOTNEST_COMPOSE_ARGS) down

debug:
	docker compose $(DOTNEST_COMPOSE_ARGS) -f docker-compose.debug.yml up --build -d

pull:
	docker compose $(DOTNEST_COMPOSE_ARGS) pull

build:
	docker compose $(DOTNEST_COMPOSE_ARGS) build

config:
	docker compose $(DOTNEST_COMPOSE_ARGS) config
