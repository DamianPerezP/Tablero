SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `BDEasyBusiness` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
CREATE SCHEMA IF NOT EXISTS `bdeasybusiness` DEFAULT CHARACTER SET utf8 ;
USE `BDEasyBusiness` ;

-- -----------------------------------------------------
-- Table `BDEasyBusiness`.`Usuarios`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BDEasyBusiness`.`Usuarios` (
  `Mail` VARCHAR(45) NOT NULL,
  `NombreUsuario` VARCHAR(45) NOT NULL,
  `ApellidoUsuario` VARCHAR(45) NOT NULL,
  `NombreEmpresa` VARCHAR(45) NOT NULL,
  `Password` VARCHAR(45) NOT NULL,
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
  `IdGrafico` INT NOT NULL,
  PRIMARY KEY (`idColumnas`),
  INDEX `IdGrafico_idx` (`IdGrafico` ASC),
  CONSTRAINT `IdGrafico`
    FOREIGN KEY (`IdGrafico`)
    REFERENCES `BDEasyBusiness`.`GraficosModelos` (`idGrafico`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `bdeasybusiness` ;

-- -----------------------------------------------------
-- Table `bdeasybusiness`.`usuarios`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bdeasybusiness`.`usuarios` (
  `Mail` VARCHAR(45) NOT NULL,
  `NombreUsuario` VARCHAR(45) NOT NULL,
  `ApellidoUsuario` VARCHAR(45) NOT NULL,
  `NombreEmpresa` VARCHAR(45) NOT NULL,
  `Password` VARCHAR(45) NOT NULL,
  `BaseDeDatos` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`Mail`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;