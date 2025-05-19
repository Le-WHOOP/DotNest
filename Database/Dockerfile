FROM mcr.microsoft.com/mssql/server:2022-latest
ENV ACCEPT_EULA=Y

COPY ./init.sh ./
COPY ./dbo/Tables/ ./init/
CMD [ "/bin/bash", "./init.sh" ]
