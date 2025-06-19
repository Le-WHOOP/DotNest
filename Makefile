.PHONY: restart debug check clean

all:
	docker compose -f docker-compose.debug.yml up --build -d

restart: clean all

check:
	dotnet test DotTest/DotTest.csproj

clean:
	docker compose down
