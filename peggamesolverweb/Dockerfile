FROM node:18-alpine AS builder

WORKDIR /app


RUN npm config set registry https://registry.npmjs.org/


COPY package.json package-lock.json ./


RUN npm cache clean --force


RUN npm ci 

COPY . .

RUN npm run build


FROM nginx:alpine AS runner

WORKDIR /usr/share/nginx/html

COPY --from=builder /app/dist .

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
