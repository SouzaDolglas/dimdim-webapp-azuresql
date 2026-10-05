-- =====================================================================
-- Projeto DimDim - DDL das tabelas (Azure SQL Database)
-- cliente 1 ---- N conta   (conta.cliente_id -> cliente.id)
-- =====================================================================

IF OBJECT_ID('dbo.conta', 'U') IS NOT NULL DROP TABLE dbo.conta;
IF OBJECT_ID('dbo.cliente', 'U') IS NOT NULL DROP TABLE dbo.cliente;
GO

CREATE TABLE dbo.cliente (
    id             INT IDENTITY(1,1) NOT NULL,
    nome           NVARCHAR(100)     NOT NULL,
    cpf            NVARCHAR(14)      NOT NULL,
    email          NVARCHAR(120)     NOT NULL,
    data_cadastro  DATETIME2         NOT NULL CONSTRAINT df_cliente_data DEFAULT SYSUTCDATETIME(),
    CONSTRAINT pk_cliente PRIMARY KEY (id),
    CONSTRAINT uq_cliente_cpf UNIQUE (cpf)
);
GO

CREATE TABLE dbo.conta (
    id             INT IDENTITY(1,1) NOT NULL,
    numero         NVARCHAR(20)      NOT NULL,
    tipo           NVARCHAR(20)      NOT NULL CONSTRAINT df_conta_tipo DEFAULT 'CORRENTE',
    saldo          DECIMAL(12,2)     NOT NULL CONSTRAINT df_conta_saldo DEFAULT 0,
    data_abertura  DATETIME2         NOT NULL CONSTRAINT df_conta_data DEFAULT SYSUTCDATETIME(),
    cliente_id     INT               NOT NULL,
    CONSTRAINT pk_conta PRIMARY KEY (id),
    CONSTRAINT uq_conta_numero UNIQUE (numero),
    CONSTRAINT ck_conta_tipo CHECK (tipo IN ('CORRENTE', 'POUPANCA', 'INVESTIMENTO')),
    CONSTRAINT fk_conta_cliente FOREIGN KEY (cliente_id)
        REFERENCES dbo.cliente (id) ON DELETE CASCADE
);
GO

CREATE INDEX ix_conta_cliente ON dbo.conta (cliente_id);
GO
