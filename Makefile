.PHONY: restart debug check clean

all:
	docker compose up --build -d

restart: clean all

debug:
	echo NOPE

check:
	dotnet test DotTest/DotTest.csproj

clean:
	docker compose down
