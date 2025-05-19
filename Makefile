.PHONY: debug check clean

all:
	docker compose up --build -d

debug:
	echo NOPE

check:
	dotnet test DotTest/DotTest.csproj

clean:
	docker compose down
