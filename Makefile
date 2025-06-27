.PHONY: up down debug

up:
	docker compose --profile nginx up --build -d --remove-orphans

down:
	docker compose --profile "*" down

debug:
	docker compose -f docker-compose.yml -f docker-compose.debug.yml up --build -d
