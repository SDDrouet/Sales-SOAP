CREATE TABLE Categories (
    id INT IDENTITY(1,1) PRIMARY KEY,
    categoryName NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX)
);

CREATE TABLE Products (
    id INT IDENTITY(1,1) PRIMARY KEY,
    productName NVARCHAR(255) NOT NULL,
    categoryId INT NOT NULL,
    unitPrice DECIMAL(10, 2) NOT NULL,
    unitInStock INT NOT NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (categoryId)
    REFERENCES Categories(id) ON DELETE CASCADE

);

-- Crear tabla 'users'
CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY, -- Llave primaria con incremento automático
    username NVARCHAR(50) NOT NULL,  -- Nombre de usuario
    password NVARCHAR(255) NOT NULL, -- Contraseña (encriptada preferiblemente)
    email NVARCHAR(100) NOT NULL,    -- Correo electrónico
    rol NVARCHAR(50) NOT NULL,        -- Rol del usuario
    status INT DEFAULT 0,              -- Estado del usuario (1 = Activo, 0 = Inactivo)
    loginAttempts INT DEFAULT 0       -- Intentos de inicio de sesión
);

-- Crear tabla 'logs'
CREATE TABLE Logs (
    id INT IDENTITY(1,1) PRIMARY KEY, -- Llave primaria con incremento automático
    userModification NVARCHAR(50) NOT NULL, -- Usuario que realizó la modificación
    description NVARCHAR(255) NOT NULL,     -- Descripción del log
    dateLog DATETIME DEFAULT GETDATE() NOT NULL     -- Fecha y hora del log con valor por defecto
);
