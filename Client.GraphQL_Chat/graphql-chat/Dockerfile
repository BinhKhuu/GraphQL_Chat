FROM node:20 AS web-base
WORKDIR /Client.GraphQL_Chat
COPY . .

FROM web-base AS web-dev
RUN npm install
RUN npm install -g @angular/cli

EXPOSE 4200
CMD ["npm", "run", "docker"]