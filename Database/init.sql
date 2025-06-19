CREATE TABLE "Users" (
    "id" SERIAL PRIMARY KEY,
    "username" VARCHAR(50) NOT NULL,
    "email" VARCHAR(100) NOT NULL,
    "hashedPassword" VARCHAR(256) NOT NULL,
    "passwordSalt" VARCHAR(256) NOT NULL
);

CREATE TABLE "Pictures" (
    "id" SERIAL PRIMARY KEY,
    "base64" TEXT NOT NULL
);

CREATE TABLE "Rentals" (
    "id" SERIAL PRIMARY KEY,
    "pictureId" INT NULL,
    "userId" INT NOT NULL,
    "name" VARCHAR(100) NOT NULL,
    "description" VARCHAR(2000) NOT NULL,
    "city" VARCHAR(100) NOT NULL,
    CONSTRAINT "FK_Rentals_Pictures" FOREIGN KEY ("pictureId") REFERENCES "Pictures"("id") ON DELETE CASCADE,
    CONSTRAINT "FK_Rentals_Users" FOREIGN KEY ("userId") REFERENCES "Users"("id") ON DELETE CASCADE
);

CREATE TABLE "Bookings" (
    "id" SERIAL PRIMARY KEY,
    "userId" INT NOT NULL,
    "rentalId" INT NOT NULL,
    "fromDate" DATE NOT NULL,
    "toDate" DATE NOT NULL,
    CONSTRAINT "FK_Bookings_Rentals" FOREIGN KEY ("rentalId") REFERENCES "Rentals"("id") ON DELETE CASCADE,
    CONSTRAINT "FK_Bookings_Users" FOREIGN KEY ("userId") REFERENCES "Users"("id")
);
