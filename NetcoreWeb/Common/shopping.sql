/*
 Navicat Premium Data Transfer

 Source Server         : ZQY
 Source Server Type    : MySQL
 Source Server Version : 50723
 Source Host           : localhost:3306
 Source Schema         : shopping

 Target Server Type    : MySQL
 Target Server Version : 50723
 File Encoding         : 65001

 Date: 17/04/2020 17:56:44
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for adminuser
-- ----------------------------
DROP TABLE IF EXISTS `adminuser`;
CREATE TABLE `adminuser`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Pwd` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Role` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '0：普通管理员，1：超级管理员\r\n1：超级管理员',
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of adminuser
-- ----------------------------
INSERT INTO `adminuser` VALUES ('12', '1', '1', '1', b'0');

-- ----------------------------
-- Table structure for appraise
-- ----------------------------
DROP TABLE IF EXISTS `appraise`;
CREATE TABLE `appraise`  (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NULL DEFAULT NULL,
  `ProductId` int(11) NULL DEFAULT NULL,
  `Content` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Grade` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `RateTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for category
-- ----------------------------
DROP TABLE IF EXISTS `category`;
CREATE TABLE `category`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CateName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ParentId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of category
-- ----------------------------
INSERT INTO `category` VALUES ('0b3f9642-36fb-4674-8284-361ab74b2381', '鞋子', '男装', b'0');
INSERT INTO `category` VALUES ('121212', '男装', '0', b'0');
INSERT INTO `category` VALUES ('121213', '女装', '0', b'0');
INSERT INTO `category` VALUES ('121218', '夏季', '0', b'0');
INSERT INTO `category` VALUES ('2ef485e3-46be-4bbe-8ee6-57c790cdca07', '2323', '121218', b'0');
INSERT INTO `category` VALUES ('50e8edc8-23e7-4012-8488-3e1c34a4fcaf', '12312', 'cd6a7357-8093-46cb-871e-b10d3b90ab2a', b'0');
INSERT INTO `category` VALUES ('631cbada-d36f-45f0-937f-0b898df79a10', '休闲娱乐', '121212', b'0');
INSERT INTO `category` VALUES ('7201e8d8-6014-493b-ac46-50e55634a323', '性感', '121218', b'0');
INSERT INTO `category` VALUES ('720954fe-45ee-4963-ac2a-428c2bc5bbeb', '23', '121218', b'0');
INSERT INTO `category` VALUES ('8f16ceb2-c162-4a69-8b60-5a84a154cb8f', '先日风情', '121212', b'0');
INSERT INTO `category` VALUES ('cd6a7357-8093-46cb-871e-b10d3b90ab2a', '121', '0', b'0');
INSERT INTO `category` VALUES ('e8d29cb9-5855-4737-bc04-571921a63814', '疯狂一下', '121212', b'0');
INSERT INTO `category` VALUES ('f27d597f-a468-4369-ba85-659b5583bb4d', '优雅', '121213', b'0');

-- ----------------------------
-- Table structure for customer
-- ----------------------------
DROP TABLE IF EXISTS `customer`;
CREATE TABLE `customer`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Pwd` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Email` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Nick` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DeliveryId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of customer
-- ----------------------------
INSERT INTO `customer` VALUES ('5069b14e-c5ce-4ab4-97aa-5341d58f2aa9', '1', '1', '121', '121', '?', b'0');
INSERT INTO `customer` VALUES ('5ea694e7-59b8-4d39-a3e7-f98387f3cbae', '@5ea694e7-59b8-4d39-a3e7-f98387f3cbae', '@UsersName', '@Nick', '@Pwd', '@Email', b'0');
INSERT INTO `customer` VALUES ('60fd5906-d36a-4fa4-b20a-4017b67d27e9', '地方', ' 地方 ', '对方的', '对方的', '?', b'0');
INSERT INTO `customer` VALUES ('6659d23c-5b4b-4e69-b7a5-d432002d6b37', NULL, NULL, NULL, NULL, '?', b'0');
INSERT INTO `customer` VALUES ('73a37079-1bd9-4df8-890c-f45a48952253', '睡得', '是多少', '是的', '睡得是的', '?', b'0');
INSERT INTO `customer` VALUES ('771ba104-1425-476e-9680-6bde60908624', '33', '23', '23', '23', '?', b'0');
INSERT INTO `customer` VALUES ('81c6588f-491f-4ce7-ac22-f08813d1db28', '是', '是的', '是的', '是的', '?', b'0');
INSERT INTO `customer` VALUES ('8bf952e0-5ff1-4b79-993a-615ac0f04a6a', '121', '12', '12', '12', '?', b'0');
INSERT INTO `customer` VALUES ('957ef35b-ce93-40f4-bf9f-16f22ff01bd9', 'asa ', 'asa ', 'asa', 'asa', '?', b'0');
INSERT INTO `customer` VALUES ('9a2341ae-bd9a-464b-9afd-2be67ab5c6ac', '是的', '是的', '为', '是的', '?', b'0');
INSERT INTO `customer` VALUES ('@Email', '@UserId', '@UsersName', '@Nick', '@Pwd', '@Email', b'0');
INSERT INTO `customer` VALUES ('@UserId', 'dfd', '@Pwd', '@Email', '@Nick', '@DeliveryId', b'0');
INSERT INTO `customer` VALUES ('a3679902-f105-47cb-a879-5af5382d5a3d', '是的', '是的', '是的', '是的', '?', b'0');
INSERT INTO `customer` VALUES ('de6ead6f-7771-4760-87db-b6b4df955dfc', '是的', '是的', '是的', '是的', '?', b'0');

-- ----------------------------
-- Table structure for delivery
-- ----------------------------
DROP TABLE IF EXISTS `delivery`;
CREATE TABLE `delivery`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Consignee` int(50) NULL DEFAULT NULL,
  `Complete` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Phone` varchar(12) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for detail
-- ----------------------------
DROP TABLE IF EXISTS `detail`;
CREATE TABLE `detail`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OrdersID` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ProductId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Quantity` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `States` int(255) NULL DEFAULT NULL COMMENT '明细状态：0正常，1退货中，2已退货',
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for favorite
-- ----------------------------
DROP TABLE IF EXISTS `favorite`;
CREATE TABLE `favorite`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProductId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UserId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for news
-- ----------------------------
DROP TABLE IF EXISTS `news`;
CREATE TABLE `news`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `NTypes` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Content` tinytext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `PhotoUrl` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PushTime` datetime(0) NULL DEFAULT NULL,
  `States` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '消息状态：0置顶，1热点',
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of news
-- ----------------------------
INSERT INTO `news` VALUES ('532b0584-e85a-4355-8451-8b055069d281', '商品名就', '23', '2323232323', NULL, '0001-01-01 00:00:00', '0', b'0');
INSERT INTO `news` VALUES ('bdb81c0a-65ce-4313-bdbb-f2759c4a5fca', '23', '231212', '23', NULL, '0001-01-01 00:00:00', '0', b'0');
INSERT INTO `news` VALUES ('bff3ea64-0f1b-4817-99e0-0e52c526ce2e', '23', '23', '23', '', '0001-01-01 00:00:00', '0', b'0');

-- ----------------------------
-- Table structure for orders
-- ----------------------------
DROP TABLE IF EXISTS `orders`;
CREATE TABLE `orders`  (
  `Id` int(11) NOT NULL,
  `Orderdate` datetime(0) NULL DEFAULT NULL,
  `UserId` int(11) NULL DEFAULT NULL,
  `Total` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DeliveryID` int(11) NULL DEFAULT NULL,
  `DeliveryDate` datetime(0) NULL DEFAULT NULL,
  `States` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '订单状态：\r\n0未付款，1已付款，2已发货，3已收货，4已评价',
  `Remark` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for photo
-- ----------------------------
DROP TABLE IF EXISTS `photo`;
CREATE TABLE `photo`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProductId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PhotoUrl` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  `IsIcon` bit(1) NULL DEFAULT NULL COMMENT '是否封面',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of photo
-- ----------------------------
INSERT INTO `photo` VALUES ('50d75b89-c435-4759-b15d-ab514af3191c', '8c1aa1a2-f572-4749-ba41-b4f4dd41ea00', '../../img/2019-08-05/4f8dfe17-4db3-4e51-bc70-38a8b16c1c8b.jpg', b'0', b'0');
INSERT INTO `photo` VALUES ('8e668401-8022-4099-a76c-60c7009947bc', 'ea076d91-a091-409f-a2af-f4347bd9f4e0', '../../img/2019-08-02/39bdd744-e5ff-4cb7-979a-2e6b11bb139a.jpg', b'0', b'0');
INSERT INTO `photo` VALUES ('fb12973a-fcff-4991-be26-d5536fabd946', 'b3e93f8e-adb9-4505-922c-b34f485e14d9', '../../img/2019-08-02/39bdd744-e5ff-4cb7-979a-2e6b11bb139a.jpg', b'0', b'0');

-- ----------------------------
-- Table structure for product
-- ----------------------------
DROP TABLE IF EXISTS `product`;
CREATE TABLE `product`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CateId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `MarketPrice` decimal(10, 2) NULL DEFAULT NULL,
  `Price` decimal(10, 2) NULL DEFAULT NULL,
  `Content` tinytext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `PostTime` datetime(0) NULL DEFAULT NULL,
  `Stock` int(11) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  `Icon` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '封面',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of product
-- ----------------------------
INSERT INTO `product` VALUES ('2843a18a-4585-4c57-b857-a95d701935cf', '23', '631cbada-d36f-45f0-937f-0b898df79a10', 23.00, 2323.00, '232', '2019-08-02 00:00:00', 23, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');
INSERT INTO `product` VALUES ('8c1aa1a2-f572-4749-ba41-b4f4dd41ea00', '1212', '631cbada-d36f-45f0-937f-0b898df79a10', 121.00, 121.00, '121212', '2019-08-05 00:00:00', 121, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');
INSERT INTO `product` VALUES ('acb3a7a8-7a73-4f9b-8125-995635c15186', '121', '8f16ceb2-c162-4a69-8b60-5a84a154cb8f', 12.00, 12.00, '12121', '2019-08-02 00:00:00', 12, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');
INSERT INTO `product` VALUES ('c6ed5e25-808c-429d-8521-3d65b2221b6a', '2323', '631cbada-d36f-45f0-937f-0b898df79a10', 2323.00, 22323.00, '32323223', '2019-08-02 00:00:00', 23, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');
INSERT INTO `product` VALUES ('ea076d91-a091-409f-a2af-f4347bd9f4e0', '男鞋', '631cbada-d36f-45f0-937f-0b898df79a10', 121.00, 121.00, '121212121212', '2019-08-02 00:00:00', 121, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');

SET FOREIGN_KEY_CHECKS = 1;
