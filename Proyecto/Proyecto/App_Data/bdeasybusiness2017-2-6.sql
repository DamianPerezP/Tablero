-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema BDEasyBusiness
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema BDEasyBusiness
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `BDEasyBusiness` DEFAULT CHARACTER SET utf8 ;
USE `BDEasyBusiness` ;

-- -----------------------------------------------------
-- Table `BDEasyBusiness`.`Usuarios`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BDEasyBusiness`.`Usuarios` (
  `Mail` VARCHAR(45) NOT NULL,
  `NombreUsuario` VARCHAR(45) NOT NULL,
  `ApellidoUsuario` VARCHAR(45) NOT NULL,
  `NombreEmpresa` VARCHAR(45) NOT NULL,
  `Contraseña` VARCHAR(45) NOT NULL,
  `BaseDeDatos` VARCHAR(45) NULL,
  `IdUsuario` INT NOT NULL AUTO_INCREMENT,
  `FechaBD` DATETIME NULL,
  PRIMARY KEY (`IdUsuario`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BDEasyBusiness`.`Tablas`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BDEasyBusiness`.`Tablas` (
  `IdTabla` INT NOT NULL AUTO_INCREMENT,
  `IdUsuario` INT NOT NULL,
  `Nombre` VARCHAR(45) NOT NULL,
  `Fecha` DATETIME NOT NULL,
  PRIMARY KEY (`IdTabla`),
  UNIQUE INDEX `IdTabla_UNIQUE` (`IdTabla` ASC),
  INDEX `IdUsuario_idx` (`IdUsuario` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `BDEasyBusiness`.`GraficosModelos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BDEasyBusiness`.`GraficosModelos` (
  `idGrafico` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NULL,
  PRIMARY KEY (`idGrafico`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BDEasyBusiness`.`Graficos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BDEasyBusiness`.`Graficos` (
  `idGrafico` INT NOT NULL,
  `Nombre` VARCHAR(45) NOT NULL,
  `Orden` INT NULL,
  `TipoGrafico` INT NOT NULL,
  PRIMARY KEY (`idGrafico`),
  INDEX `TipoGrafico_idx` (`TipoGrafico` ASC),
  CONSTRAINT `TipoGrafico`
    FOREIGN KEY (`TipoGrafico`)
    REFERENCES `BDEasyBusiness`.`GraficosModelos` (`idGrafico`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BDEasyBusiness`.`ContenidoTablas`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BDEasyBusiness`.`ContenidoTablas` (
  `idContenidoTablas` INT NOT NULL AUTO_INCREMENT,
  `IdTabla` INT NOT NULL,
  `IdGrafico` INT NOT NULL,
  PRIMARY KEY (`idContenidoTablas`),
  INDEX `IdTabla_idx` (`IdTabla` ASC),
  INDEX `IdGrafico_idx` (`IdGrafico` ASC),
  CONSTRAINT `IdTabla`
    FOREIGN KEY (`IdTabla`)
    REFERENCES `BDEasyBusiness`.`Tablas` (`IdTabla`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdGrafico`
    FOREIGN KEY (`IdGrafico`)
    REFERENCES `BDEasyBusiness`.`Graficos` (`idGrafico`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BDEasyBusiness`.`Columnas`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BDEasyBusiness`.`Columnas` (
  `idColumnas` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  `idGrafico` INT NOT NULL,
  PRIMARY KEY (`idColumnas`),
  INDEX `idGrafico_idx` (`idGrafico` ASC),
  CONSTRAINT `idGrafico`
    FOREIGN KEY (`idGrafico`)
    REFERENCES `BDEasyBusiness`.`GraficosModelos` (`idGrafico`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

DROP PROCEDURE IF EXISTS CrearBD;
DROP PROCEDURE IF EXISTS Registrar;
DROP PROCEDURE IF EXISTS Login;
DROP PROCEDURE IF EXISTS TraerBaseDeDatos;
DROP PROCEDURE IF EXISTS TraerUsuario;

DELIMITER $$
CREATE PROCEDURE CrearBD()
BEGIN
UPDATE `usuarios` SET `BaseDeDatos` = 'ParBaseDeDatos'
WHERE `Mail` = 'ParMail';
END
$$

DELIMITER $$
CREATE PROCEDURE Registrar()
BEGIN
INSERT INTO `usuarios` ( `NombreUsuario`, `Apellido`, `Mail`, `NombreEmpresa`, `Contraseña` )
VALUES ('ParNombreUsuario', 'ParApellido', 'ParMail', 'ParNombreEmpresa', 'ParContrasena');
END
$$

DELIMITER $$
CREATE PROCEDURE Login()
BEGIN
SELECT `Mail`
FROM `usuarios`
WHERE `Mail` = 'ParMail' AND Contraseña = 'ParContrasena';
END
$$

DELIMITER $$
CREATE PROCEDURE TraerBaseDeDatos()
BEGIN
SELECT `BaseDeDatos`
FROM `usuarios`
WHERE `Mail` = 'ParMail';
END
$$

DELIMITER $$
CREATE PROCEDURE TraerUsuario()
BEGIN
SELECT *
FROM `usuarios`
WHERE `Mail` = 'ParMaiil';
END
$$



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
