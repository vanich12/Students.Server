# Makefile

install: # Установка зависимостей
	npm ci

build: # Сборка
	npm run build

start: #  Запуск приложения (на localhost:3000)
	npm start

test: #  Запуск тестов (Jest)
	npm test

lint: #  Запуск линтера
	npx eslint

server: #  Запуск тестового сервера
	node test_server/server.js

test: #  Запуск тестов (Jest)
	npm test

dockerBuild: 
	docker build -t cyfralab-app .

dockerStart:
	docker run -p 4000:3000 cyfralab-app
