CREATE DATABASE  IF NOT EXISTS `control_tortas` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_spanish_ci */;
USE `control_tortas`;
-- MySQL dump 10.13  Distrib 5.6.17, for Win32 (x86)
--
-- Host: localhost    Database: control_tortas
-- ------------------------------------------------------
-- Server version	5.0.51b-community-nt-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Not dumping tablespaces as no INFORMATION_SCHEMA.FILES table on this server
--

--
-- Table structure for table `articulos`
--

DROP TABLE IF EXISTS `articulos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `articulos` (
  `clave` varchar(7) collate utf8_spanish_ci NOT NULL,
  `articulos` varchar(100) collate utf8_spanish_ci NOT NULL,
  PRIMARY KEY  (`clave`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci COMMENT='Descripcion de los articulos';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `articulos`
--

LOCK TABLES `articulos` WRITE;
/*!40000 ALTER TABLE `articulos` DISABLE KEYS */;
INSERT INTO `articulos` VALUES ('980103','TORTA DE ARRACHERA EMPACADA AL VACIO'),('980104','TORTA DE ADOBADA EMPACADA AL VACIO'),('980105','TORTA DE PECHUGA EMPACADA AL VACIO'),('980106','TORTA DE PIERNA EMPACADA AL VACIO'),('980107','TORTA DE 3 QUESOS CORRALES EMPACADA AL VACIO'),('980111','TORTA DIABLONA EMPACADA AL VACIO'),('980119','TORTA DE MILANESA EMPACADA AL VACIO'),('980120','TORTA DE ADOBADA CON QUESO EMPACADA AL VACIO'),('980122','TORTA DE HAMBURGUESA EMPACADA AL VACIO'),('980202','TACO DE MACIZA EMPACADO AL VACIO'),('980203','TACO DE SURTIDO EMPACADO AL VACIO'),('980204','TACO DE YESCA EMPACADO AL VACIO');
/*!40000 ALTER TABLE `articulos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ventas`
--

DROP TABLE IF EXISTS `ventas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ventas` (
  `folio_ticket` varchar(10) collate utf8_spanish_ci NOT NULL,
  `fecha_ticket` date NOT NULL,
  `hora_ticket` time NOT NULL,
  `cantidad_ticket` smallint(5) unsigned NOT NULL,
  `clave_articulo` varchar(7) collate utf8_spanish_ci NOT NULL,
  PRIMARY KEY  (`folio_ticket`,`fecha_ticket`,`hora_ticket`,`cantidad_ticket`,`clave_articulo`),
  KEY `fff_idx` (`clave_articulo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci COMMENT='Control de las ventas de tortas';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ventas`
--

LOCK TABLES `ventas` WRITE;
/*!40000 ALTER TABLE `ventas` DISABLE KEYS */;
/*!40000 ALTER TABLE `ventas` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2014-12-06  9:12:04
