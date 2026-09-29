-- DimDim - DDL das tabelas (Azure SQL Database)
IF OBJECT_ID('dbo.Transacao') IS NOT NULL DROP TABLE dbo.Transacao;
IF OBJECT_ID('dbo.Cliente')   IS NOT NULL DROP TABLE dbo.Cliente;

CREATE TABLE dbo.Cliente (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Nome          NVARCHAR(100) NOT NULL,
    Email         NVARCHAR(150) NOT NULL,
    DataCadastro  DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT UQ_Cliente_Email UNIQUE (Email)
);

CREATE TABLE dbo.Transacao (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId      INT           NOT NULL,
    Valor          DECIMAL(12,2) NOT NULL,
    Descricao      NVARCHAR(200) NULL,
    DataTransacao  DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Transacao_Cliente FOREIGN KEY (ClienteId)
        REFERENCES dbo.Cliente (Id) ON DELETE CASCADE
);
