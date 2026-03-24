-- MySQL dump 10.13  Distrib 8.0.43, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: stankidb
-- ------------------------------------------------------
-- Server version	8.0.43

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
-- Table structure for table `machine_loads`
--

DROP TABLE IF EXISTS `machine_loads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `machine_loads` (
  `id_ml` int NOT NULL AUTO_INCREMENT,
  `temperature` float DEFAULT NULL,
  `rotation` float DEFAULT NULL,
  `time` datetime DEFAULT NULL,
  `id_mn` int DEFAULT NULL,
  PRIMARY KEY (`id_ml`),
  KEY `id_mn_idx` (`id_mn`),
  CONSTRAINT `fk_test_connection` FOREIGN KEY (`id_mn`) REFERENCES `machine_names` (`id_mn`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_loads`
--

LOCK TABLES `machine_loads` WRITE;
/*!40000 ALTER TABLE `machine_loads` DISABLE KEYS */;
INSERT INTO `machine_loads` VALUES (1,22.4,1000,'2026-03-24 08:00:00',1),(2,23.1,1000,'2026-03-24 08:01:00',1),(3,24.2,2500,'2026-03-24 08:02:00',1),(4,26.8,5000,'2026-03-24 08:03:00',1),(5,30.5,5000,'2026-03-24 08:04:00',1),(6,35.2,8000,'2026-03-24 08:05:00',1),(7,41,8000,'2026-03-24 08:06:00',1),(8,46.3,10000,'2026-03-24 08:07:00',1),(9,50.5,12000,'2026-03-24 08:08:00',1),(10,53.2,12000,'2026-03-24 08:09:00',1),(11,55.1,12000,'2026-03-24 08:10:00',1),(12,56.4,12000,'2026-03-24 08:11:00',1),(13,57.2,12000,'2026-03-24 08:12:00',1),(14,57.8,12000,'2026-03-24 08:13:00',1),(15,58.1,12000,'2026-03-24 08:14:00',1),(16,21,2000,'2026-03-24 09:00:00',2),(17,24.5,5000,'2026-03-24 09:05:00',2),(18,32.8,10000,'2026-03-24 09:10:00',2),(19,45.1,15000,'2026-03-24 09:15:00',2),(20,52.4,18000,'2026-03-24 09:20:00',2),(21,20.5,1000,'2026-03-24 08:30:00',3),(22,22.8,4000,'2026-03-24 08:40:00',3),(23,28.2,8000,'2026-03-24 08:50:00',3),(24,35.5,12000,'2026-03-24 09:00:00',3),(25,41.9,16000,'2026-03-24 09:10:00',3),(26,19.8,500,'2026-03-24 07:00:00',4),(27,21.2,1500,'2026-03-24 07:15:00',4),(28,25.4,3000,'2026-03-24 07:30:00',4),(29,31,4500,'2026-03-24 07:45:00',4),(30,36.7,6000,'2026-03-24 08:00:00',4),(31,23,5000,'2026-03-24 10:00:00',5),(32,29.5,12000,'2026-03-24 10:02:00',5),(33,42.1,24000,'2026-03-24 10:04:00',5),(34,55.8,36000,'2026-03-24 10:06:00',5),(35,64.2,42000,'2026-03-24 10:08:00',5),(36,20.1,0,'2026-03-24 11:00:00',6),(37,21.5,50,'2026-03-24 11:10:00',6),(38,22.8,50,'2026-03-24 11:20:00',6),(39,23.4,50,'2026-03-24 11:30:00',6),(40,23.9,50,'2026-03-24 11:40:00',6);
/*!40000 ALTER TABLE `machine_loads` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `machine_names`
--

DROP TABLE IF EXISTS `machine_names`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `machine_names` (
  `id_mn` int NOT NULL AUTO_INCREMENT,
  `name` tinytext,
  `manufacturer` tinytext,
  `id_mt` int DEFAULT NULL,
  PRIMARY KEY (`id_mn`),
  KEY `id_mt_idx` (`id_mt`) /*!80000 INVISIBLE */,
  CONSTRAINT `id_mt` FOREIGN KEY (`id_mt`) REFERENCES `machine_types` (`id_mt`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_names`
--

LOCK TABLES `machine_names` WRITE;
/*!40000 ALTER TABLE `machine_names` DISABLE KEYS */;
INSERT INTO `machine_names` VALUES (1,'VF-2SS','Haas',1),(2,'DMP 70','DMG MORI',1),(3,'C 42 U','Hermle',2),(4,'NLX 2500','DMG MORI',3),(5,'MILL S 400','GF Mikron',4),(6,'FORM P 350','AgieCharmilles',5);
/*!40000 ALTER TABLE `machine_names` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `machine_states`
--

DROP TABLE IF EXISTS `machine_states`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `machine_states` (
  `id_ms` int NOT NULL AUTO_INCREMENT,
  `name` tinytext,
  `status` tinytext,
  `value` tinytext,
  `description` tinytext,
  `id_mn` int DEFAULT NULL,
  PRIMARY KEY (`id_ms`),
  KEY `id_mn_idx` (`id_mn`),
  CONSTRAINT `id_mn` FOREIGN KEY (`id_mn`) REFERENCES `machine_names` (`id_mn`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_states`
--

LOCK TABLES `machine_states` WRITE;
/*!40000 ALTER TABLE `machine_states` DISABLE KEYS */;
INSERT INTO `machine_states` VALUES (1,'Цикл завершен','SUCCESS','12000','Рабочая температура шпинделя достигнута',1),(2,'Запуск G25','INFO','2000','Активация макроса прогрева шпинделя',2),(3,'Контроль СОЖ','INFO','22.0','Температура охлаждающей жидкости в норме',2),(4,'Готов к работе','SUCCESS','18000','Оптимальный тепловой зазор достигнут',2),(5,'Проверка осей','INFO','0','Калибровка измерительного щупа перед прогревом',3),(6,'Термо-пауза','WARNING','35.0','Остановка для стабилизации вылета шпинделя',3),(7,'Статус: OK','SUCCESS','16000','Геометрия станка стабилизирована',3),(8,'Прогрев патрона','INFO','500','Вращение главного шпинделя на малых оборотах',4),(9,'Ток привода','INFO','12.5','Потребление энергии в пределах нормы',4),(10,'Прогрев завершен','SUCCESS','6000','Масло в редукторе достигло рабочей вязкости',4),(11,'Oil-Air Lubrication','INFO','1','Проверка подачи масляного тумана в подшипники',5),(12,'Балансировка','SUCCESS','0.01','Допустимый уровень биения на 42000 об/мин',5),(13,'High-Speed Ready','SUCCESS','42000','Шпиндель готов к высокоскоростной обработке',5),(14,'Промывка','INFO','0','Очистка рабочей зоны диэлектриком',6),(15,'Стабилизация Т','SUCCESS','24.0','Температура ванны стабилизирована',6);
/*!40000 ALTER TABLE `machine_states` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `machine_types`
--

DROP TABLE IF EXISTS `machine_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `machine_types` (
  `id_mt` int NOT NULL AUTO_INCREMENT,
  `type` tinytext,
  PRIMARY KEY (`id_mt`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_types`
--

LOCK TABLES `machine_types` WRITE;
/*!40000 ALTER TABLE `machine_types` DISABLE KEYS */;
INSERT INTO `machine_types` VALUES (1,'Вертикально-фрезерный ОЦ'),(2,'Пятиосевой обрабатывающий центр'),(3,'Токарно-фрезерный центр'),(4,'Высокоскоростной микрофрезерный станок'),(5,'Электроэрозионный прошивочный станок');
/*!40000 ALTER TABLE `machine_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'stankidb'
--
/*!50003 DROP PROCEDURE IF EXISTS `GetMachineLoadHistory` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMachineLoadHistory`(IN target_id INT)
BEGIN
	SELECT `time`, `temperature`, `rotation`
    FROM `machine_loads`
    WHERE `id_mn` = target_id
    ORDER BY `time` ASC;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetMachineLoadHistoryByPeriod` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMachineLoadHistoryByPeriod`(
    IN target_id INT, 
    IN t1 DATETIME, 
    IN t2 DATETIME
)
BEGIN
	SELECT `time`, `temperature`, `rotation`
    FROM `machine_loads`
    WHERE `id_mn` = target_id 
      AND `time` BETWEEN t1 AND t2
    ORDER BY `time` ASC;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetMachineStatusLogs` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMachineStatusLogs`(IN target_id INT)
BEGIN
	SELECT `name`, `status`, `value`, `description`
    FROM `machine_states`
    WHERE `id_mn` = target_id
    ORDER BY `id_ms` DESC;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetModelList` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetModelList`()
BEGIN
	SELECT `id_mn`, `name` FROM `machine_names`;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetTypesList` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetTypesList`()
BEGIN
	SELECT `id_type`, `type_name` FROM `machine_types`;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-24 13:45:27
