CREATE DATABASE IF NOT EXISTS DbGimnasio;
USE DbGimnasio;

-- Tabla de Usuarios (Socios)
CREATE TABLE Usuarios(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Edad INT NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    IsActive BIT DEFAULT 1,
    FechaRegistro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de Membresías
CREATE TABLE Membresias (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Descripcion VARCHAR(20),
    Precio DECIMAL(10,2) NOT NULL,
    DuracionDias INT NOT NULL,
    ClasesIncluidas INT DEFAULT 0,
    IsActive BIT DEFAULT 1
);

-- Tabla de Usuario_Membresias
CREATE TABLE Usuario_Membresias (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT NOT NULL,
    MembresiaId INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    ClasesRestantes INT DEFAULT 0,
    Estado ENUM('Activa', 'Expirada', 'Cancelada') NOT NULL,
    PrecioPagado DECIMAL(10,2) NOT NULL,
    MetodoPago ENUM('Efectivo', 'Tarjeta', 'QR') NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (MembresiaId) REFERENCES Membresias(Id)
);

-- Tabla de Instructores
CREATE TABLE Instructores (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Especialidad VARCHAR(100),
    Telefono VARCHAR(20),
    IsActive BIT DEFAULT 1
);

-- Tabla de Clases
CREATE TABLE Clases (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Descripcion VARCHAR(30),
    InstructorId INT NOT NULL,
    CapacidadMaxima INT NOT NULL,
    DuracionMinutos INT DEFAULT 60,
    Nivel ENUM('Principiante', 'Intermedio', 'Avanzado') DEFAULT 'Principiante',
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (InstructorId) REFERENCES Instructores(Id)
);

-- Tabla de Horarios
CREATE TABLE Horarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ClaseId INT NOT NULL,
    DiaSemana ENUM('Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes') NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    Sala VARCHAR(10),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (ClaseId) REFERENCES Clases(Id)
);

-- Tabla de Asistencia
CREATE TABLE Asistencia (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT NOT NULL,
    HorarioId INT NOT NULL,
    FechaAsistencia DATE NOT NULL,
    Estado ENUM('Presente', 'Ausente') DEFAULT 'Presente',
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (HorarioId) REFERENCES Horarios(Id)
);

-- Insertar datos de ejemplo

-- Usuarios
INSERT INTO Usuarios (Nombre, Edad, Telefono) VALUES
('Chris Ferrufino', 20, '11-1111-1111'),
('Rafael Cenzano', 25, '11-2222-2222'),
('Axel Fernandez', 30, '11-3333-3333');

-- Membresías
INSERT INTO Membresias (Descripcion, Precio, DuracionDias, ClasesIncluidas) VALUES
('Acceso al gimnasio', 200.00, 30, 0),
('8 clases incluidas', 50.00, 30, 15),
('Clases Infinitas', 7000.00, 365, 999);

-- Membresías de usuarios
INSERT INTO Usuario_Membresias (UsuarioId, MembresiaId, FechaInicio, FechaFin, ClasesRestantes, Estado, PrecioPagado, MetodoPago) VALUES
(1, 2, '2024-01-01', '2025-01-31', 15, 'Activa', 50.00, 'Efectivo'),
(2, 3, '2024-01-01', '2026-12-31', 999, 'Activa', 7000.00, 'Tarjeta'),
(3, 1, '2024-10-01', '2026-01-31', 3, 'Activa', 200.00, 'QR');

-- Instructores
INSERT INTO Instructores (Nombre, Especialidad, Telefono) VALUES
('Sergio Gómez', 'Boxeo', '11-1234-5678'),
('Alejandro Rodríguez', 'Boxeo', '11-2345-6789'),
('Rodrigo Rodríguez', 'Muay Thai', '11-3456-7890');

-- Clases
INSERT INTO Clases (Descripcion, InstructorId, CapacidadMaxima, DuracionMinutos, Nivel) VALUES
('Boxeo Principiantes', 1, 20, 60, 'Principiante'),
('Boxeo Avanzado', 1, 15, 90, 'Avanzado'),
('Muay Thai', 3, 10, 90, 'Avanzado');

-- Horarios
INSERT INTO Horarios (ClaseId, DiaSemana, HoraInicio, HoraFin, Sala) VALUES
(1, 'Lunes', '18:00:00', '19:00:00', 'Sala A'),
(1, 'Miércoles', '18:00:00', '19:00:00', 'Sala A'),
(1, 'Viernes', '18:00:00', '19:00:00', 'Sala A'),
(2, 'Martes', '19:00:00', '20:30:00', 'Sala A'),
(2, 'Jueves', '19:00:00', '20:30:00', 'Sala A'),
(3, 'Lunes', '20:00:00', '21:00:00', 'Sala B'),
(3, 'Miércoles', '20:00:00', '21:00:00', 'Sala B');

-- Asistencia de ejemplo
INSERT INTO Asistencia (UsuarioId, HorarioId, FechaAsistencia, Estado) VALUES
(1, 1, '2025-01-15', 'Presente'),
(1, 2, '2025-01-17', 'Presente'),
(2, 4, '2025-01-16', 'Presente');
