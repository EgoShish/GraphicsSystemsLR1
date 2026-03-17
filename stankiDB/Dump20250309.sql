CREATE DATABASE  IF NOT EXISTS `dashboard` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `dashboard`;
-- MySQL dump 10.13  Distrib 8.0.27, for Win64 (x86_64)
--
-- Host: localhost    Database: dashboard
-- ------------------------------------------------------
-- Server version	8.0.27

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `machine_tool_load`
--

DROP TABLE IF EXISTS `machine_tool_load`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `machine_tool_load` (
  `id_mtl` int NOT NULL AUTO_INCREMENT,
  `status` varchar(45) DEFAULT NULL,
  `time_mtl` varchar(45) DEFAULT NULL,
  `id_mtn` int DEFAULT NULL,
  PRIMARY KEY (`id_mtl`),
  KEY `id_mtn_load_idx` (`id_mtn`),
  CONSTRAINT `id_mtn_load` FOREIGN KEY (`id_mtn`) REFERENCES `machine_tool_name` (`id_mtn`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=187 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_tool_load`
--

LOCK TABLES `machine_tool_load` WRITE;
/*!40000 ALTER TABLE `machine_tool_load` DISABLE KEYS */;
INSERT INTO `machine_tool_load` VALUES (1,'off','15',1),(2,'off','15',1),(3,'off','60',1),(4,'off','60',1),(5,'off','60',1),(6,'off','60',1),(7,'off','180',1),(8,'on','15',1),(9,'on','15',1),(10,'load','30',1),(11,'load','15',1),(12,'load','20',1),(13,'load','15',1),(14,'load','30',1),(15,'load','15',1),(16,'on','30',1),(17,'on','30',1),(18,'on','5',1),(19,'load','30',1),(20,'load','30',1),(21,'load','60',1),(22,'load','15',1),(23,'load','15',1),(24,'load','15',1),(25,'load','20',1),(26,'load','20',1),(27,'load','15',1),(28,'load','30',1),(29,'load','15',1),(30,'load','30',1),(31,'on','20',1),(32,'on','10',1),(33,'on','5',1),(34,'on','2',1),(35,'on','2',1),(36,'on','2',1),(37,'load','15',1),(38,'load','10',1),(39,'on','2',1),(40,'on','2',1),(41,'load','12',1),(42,'on','5',1),(43,'load','15',1),(44,'on','2',1),(45,'on','10',1),(46,'off','30',1),(47,'off','30',1),(48,'off','30',1),(49,'off','30',1),(50,'on','20',1),(51,'on','30',1),(52,'on','30',1),(53,'on','15',1),(54,'on','20',1),(55,'load','30',1),(56,'load','15',1),(57,'on','20',1),(58,'on','15',1),(59,'on','21',1),(60,'off','15',1),(61,'load','5',1),(62,'off','5',1),(63,'off','15',2),(64,'off','15',2),(65,'off','60',2),(66,'off','60',2),(67,'off','60',2),(68,'off','60',2),(69,'off','180',2),(70,'on','15',2),(71,'on','15',2),(72,'load','30',2),(73,'load','15',2),(74,'load','20',2),(75,'load','15',2),(76,'load','30',2),(77,'load','15',2),(78,'on','30',2),(79,'on','30',2),(80,'on','5',2),(81,'load','30',2),(82,'load','30',2),(83,'load','60',2),(84,'load','15',2),(85,'load','15',2),(86,'load','15',2),(87,'load','20',2),(88,'load','20',2),(89,'load','15',2),(90,'load','30',2),(91,'load','15',2),(92,'load','30',2),(93,'on','12',2),(94,'on','10',2),(95,'on','15',2),(96,'on','2',2),(97,'on','2',2),(98,'on','2',2),(99,'load','15',2),(100,'load','10',2),(101,'on','2',2),(102,'on','2',2),(103,'load','30',2),(104,'on','5',2),(105,'load','15',2),(106,'on','2',2),(107,'on','10',2),(108,'off','30',2),(109,'off','15',2),(110,'off','30',2),(111,'off','30',2),(112,'on','20',2),(113,'on','30',2),(114,'on','30',2),(115,'on','30',2),(116,'on','20',2),(117,'load','30',2),(118,'load','15',2),(119,'on','10',2),(120,'on','15',2),(121,'on','21',2),(122,'off','25',2),(123,'load','5',2),(124,'off','5',2),(125,'off','15',3),(126,'off','15',3),(127,'off','60',3),(128,'off','60',3),(129,'off','60',3),(130,'off','60',3),(131,'off','180',3),(132,'on','15',3),(133,'on','15',3),(134,'load','30',3),(135,'load','15',3),(136,'load','20',3),(137,'load','15',3),(138,'load','30',3),(139,'load','15',3),(140,'on','30',3),(141,'on','30',3),(142,'on','5',3),(143,'load','30',3),(144,'load','30',3),(145,'load','60',3),(146,'load','15',3),(147,'load','15',3),(148,'load','15',3),(149,'load','20',3),(150,'load','20',3),(151,'load','15',3),(152,'load','30',3),(153,'load','15',3),(154,'load','30',3),(155,'on','12',3),(156,'on','10',3),(157,'on','15',3),(158,'on','2',3),(159,'on','2',3),(160,'on','2',3),(161,'load','15',3),(162,'load','10',3),(163,'on','2',3),(164,'on','2',3),(165,'load','30',3),(166,'on','5',3),(167,'load','15',3),(168,'on','2',3),(169,'on','10',3),(170,'off','30',3),(171,'off','15',3),(172,'off','30',3),(173,'off','30',3),(174,'on','20',3),(175,'on','30',3),(176,'on','30',3),(177,'on','30',3),(178,'on','20',3),(179,'load','10',3),(180,'load','15',3),(181,'on','30',3),(182,'on','15',3),(183,'on','21',3),(184,'off','35',3),(185,'load','15',3),(186,'off','5',3);
/*!40000 ALTER TABLE `machine_tool_load` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `machine_tool_name`
--

DROP TABLE IF EXISTS `machine_tool_name`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `machine_tool_name` (
  `id_mtn` int NOT NULL,
  `machine_tool_name` tinytext,
  `id_mt` int DEFAULT NULL,
  PRIMARY KEY (`id_mtn`),
  KEY `id_mt_idx` (`id_mt`),
  CONSTRAINT `id_mt` FOREIGN KEY (`id_mt`) REFERENCES `machine_tool_type` (`id_mt`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_tool_name`
--

LOCK TABLES `machine_tool_name` WRITE;
/*!40000 ALTER TABLE `machine_tool_name` DISABLE KEYS */;
INSERT INTO `machine_tool_name` VALUES (1,'KSU1538',1),(2,'SVG124F',2),(3,'FLK1547JK7',1),(4,'DCG14HG7',2),(5,'DCH1458',3),(6,'RTCG1547',4),(7,'LOKD458',1),(8,'SKN457',1),(9,'ASD125',1),(10,'QWSC895',2),(11,'TRE1547G',3);
/*!40000 ALTER TABLE `machine_tool_name` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `machine_tool_state`
--

DROP TABLE IF EXISTS `machine_tool_state`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `machine_tool_state` (
  `id` int NOT NULL,
  `name` tinytext,
  `status` tinytext,
  `value` tinytext,
  `description` tinytext,
  `id_mtn` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_mtn_idx` (`id_mtn`),
  CONSTRAINT `id_mtn` FOREIGN KEY (`id_mtn`) REFERENCES `machine_tool_name` (`id_mtn`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_tool_state`
--

LOCK TABLES `machine_tool_state` WRITE;
/*!40000 ALTER TABLE `machine_tool_state` DISABLE KEYS */;
INSERT INTO `machine_tool_state` VALUES (1,'Аварийный останов','high','2000','Останов приводов',1),(2,'Ошибка интерполятора','high','2001','Строка G02 X12 Y15 R20 - траектория не может быть построена',2),(3,'Актуальный режим работы JOG','common','1004','Переключение режима работы',1),(4,'Ошибка ПЛК','high','3001','Сконфигурировать входы/выходы ПЛК',2),(5,'Ошибка интерполятора','high','2001','Строка G02 X12 Y15 R20 - траектория не может быть построена',1),(6,'Актуальный режим работы MDI','common','1003','Переключение режима работы',1),(7,'Ошибка интерпретатора','high','2003','команда G01 x10 Y10 R15 не может иметь такие параметры. Сконфигурирровать команду',1),(8,'Актуальный режим работы Auto','common','1001','Переключение режима работы',2),(9,'Ошибка приводов','high','2005','Сконфигурировать приводы',1),(10,'Нет связи с приводами','high','2006','Приводы не подключены. проверить подключение, сконфигурировать приводы',3),(11,'Готовность к работе','common','0000','Приводы готовы к работе',3);
/*!40000 ALTER TABLE `machine_tool_state` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `machine_tool_type`
--

DROP TABLE IF EXISTS `machine_tool_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `machine_tool_type` (
  `id_mt` int NOT NULL,
  `type` tinytext,
  PRIMARY KEY (`id_mt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_tool_type`
--

LOCK TABLES `machine_tool_type` WRITE;
/*!40000 ALTER TABLE `machine_tool_type` DISABLE KEYS */;
INSERT INTO `machine_tool_type` VALUES (1,'3-координатный вертикально-фрезерный'),(2,'токарный расточной'),(3,'токарный с фрезерной головкой'),(4,'вертикально-фрезерный с поворотным столом и поворотной головой'),(5,'токарный обрабатывающий центр'),(6,'токарный с противошпинделем'),(7,'токарный с противошпинделем и фрезерной головкой'),(8,'вертикально-фрезерный с поворотным столом');
/*!40000 ALTER TABLE `machine_tool_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'dashboard'
--

--
-- Dumping routines for database 'dashboard'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-09 16:19:11
