# fluentd/Dockerfile
FROM fluent/fluentd:v1.14.6-1.1
USER root
COPY ./wait-for-it.sh .
RUN ls
RUN chmod 777 ./wait-for-it.sh
RUN ["gem", "install", "fluent-plugin-elasticsearch", "--version", "5.2.0"]
USER fluent