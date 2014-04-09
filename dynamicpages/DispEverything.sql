CREATE TABLE DispClass (
	idDispClass INT IDENTITY NOT NULL PRIMARY KEY,
	name NVARCHAR(45)
);

CREATE TABLE DispAttribute(
	idDispAttribute INT IDENTITY NOT NULL PRIMARY KEY,
	idDispClass INT CONSTRAINT FK_DispClass REFERENCES DispClass(idDispClass),
	name NVARCHAR(45),
	SQLType NVARCHAR(45),
	formType NVARCHAR(45)
);