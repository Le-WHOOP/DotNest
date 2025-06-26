CURRENT_DIR := $(dir $(lastword $(MAKEFILE_LIST)))
DOTNEST_COMPOSE_FILES := DotNest/docker-compose.yml Database/docker-compose.yml docker-compose.yml
DOTNEST_COMPOSE_ARGS := $(foreach file, $(DOTNEST_COMPOSE_FILES), -f $(CURRENT_DIR)/$(file))
