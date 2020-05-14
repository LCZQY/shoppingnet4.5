/*
 Navicat Premium Data Transfer

 Source Server         : mysql-matser
 Source Server Type    : MySQL
 Source Server Version : 80020
 Source Host           : localhost:3306
 Source Schema         : zqy

 Target Server Type    : MySQL
 Target Server Version : 80020
 File Encoding         : 65001

 Date: 14/05/2020 16:41:55
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for zqy_ad_adverising
-- ----------------------------
DROP TABLE IF EXISTS `zqy_ad_adverising`;
CREATE TABLE `zqy_ad_adverising`  (
  `Id` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '标识',
  `SpaceId` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告位标识',
  `Cover` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告封面',
  `AdName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告名称',
  `StartTime` datetime(0) NULL DEFAULT NULL COMMENT '广告开始时间',
  `EndTime` datetime(0) NULL DEFAULT NULL COMMENT '结束时间',
  `JumpType` tinyint(0) NULL DEFAULT NULL COMMENT '跳转类型（1-应用内网页跳转，2-站外跳转，3-应用内数据跳转）',
  `Link` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告连接',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `CreateUser` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `IsDelete` bit(1) NULL DEFAULT NULL COMMENT '删除状态（0-未删除，1-已删除）',
  `CreateUserDeptId` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人部门ID(创建人组织ID)',
  `AdContent` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '' COMMENT 'JumpType 为2 此属性不能为空,表示为楼盘名字',
  `BlocId` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '集团标识',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_ad_area
-- ----------------------------
DROP TABLE IF EXISTS `zqy_ad_area`;
CREATE TABLE `zqy_ad_area`  (
  `Id` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '标识',
  `AdId` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '广告标识',
  `CityId` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '区域标识',
  `CityName` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '城市名称',
  `Sort` int(0) NULL DEFAULT NULL COMMENT '序号',
  `IsDelete` bit(1) NULL DEFAULT NULL COMMENT '是否删除',
  `BlocId` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '集团标识',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_ad_space
-- ----------------------------
DROP TABLE IF EXISTS `zqy_ad_space`;
CREATE TABLE `zqy_ad_space`  (
  `Id` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '广告位标识',
  `SpaceName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '位置名称',
  `Cover` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '封面图路径',
  `Site` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '所在位置',
  `App` tinyint(0) NULL DEFAULT NULL COMMENT '所在应用（1-经纪人APP），如：APP，WEB',
  `Width` int(0) NULL DEFAULT NULL COMMENT '广告图宽',
  `Height` int(0) NULL DEFAULT NULL COMMENT '广告图高',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `CreateUser` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '创建人',
  `IsDelete` bit(1) NULL DEFAULT NULL COMMENT '删除状态（1-删除，0-未删除）',
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '广告位Code',
  `IsNeedImg` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否必传图片',
  `BlocId` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '集团标识',
  PRIMARY KEY (`Id`, `Code`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_ad_visitorlogs
-- ----------------------------
DROP TABLE IF EXISTS `zqy_ad_visitorlogs`;
CREATE TABLE `zqy_ad_visitorlogs`  (
  `Id` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '访问标识',
  `AdId` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '广告标识',
  `UserId` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '访问用户',
  `App` tinyint(0) NULL DEFAULT NULL COMMENT '1-经纪人端，2-员工端',
  `Source` tinyint(0) NULL DEFAULT NULL COMMENT '访问来源(1-IOS，2-android，3-WEB)',
  `CityId` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '城市来源标识',
  `VisitTime` datetime(0) NULL DEFAULT NULL COMMENT '访问时间',
  `MAC` varchar(127) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '设备物理地址',
  `IPAddress` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '设备网络地址',
  `ViewDuration` int(0) NULL DEFAULT NULL COMMENT '观看时长（单位：秒）',
  `BlocId` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '集团标识',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_adminuser
-- ----------------------------
DROP TABLE IF EXISTS `zqy_adminuser`;
CREATE TABLE `zqy_adminuser`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Pwd` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Role` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '0：普通管理员，1：超级管理员\r\n1：超级管理员',
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of zqy_adminuser
-- ----------------------------
INSERT INTO `zqy_adminuser` VALUES ('12', '1', '1', '1', b'0');

-- ----------------------------
-- Table structure for zqy_appraise
-- ----------------------------
DROP TABLE IF EXISTS `zqy_appraise`;
CREATE TABLE `zqy_appraise`  (
  `Id` int(0) NOT NULL,
  `UserId` int(0) NULL DEFAULT NULL,
  `ProductId` int(0) NULL DEFAULT NULL,
  `Content` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Grade` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `RateTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_category
-- ----------------------------
DROP TABLE IF EXISTS `zqy_category`;
CREATE TABLE `zqy_category`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CateName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ParentId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of zqy_category
-- ----------------------------
INSERT INTO `zqy_category` VALUES ('072518f7-e963-4d5d-b68e-854d4f3df142', '12羞羞14545', '0', b'1');
INSERT INTO `zqy_category` VALUES ('0b3f9642-36fb-4674-8284-361ab74b2381', '鞋子', '男装', b'0');
INSERT INTO `zqy_category` VALUES ('121212', '男装', '0', b'0');
INSERT INTO `zqy_category` VALUES ('121213', '女装', '0', b'0');
INSERT INTO `zqy_category` VALUES ('121218', '夏季', '0', b'0');
INSERT INTO `zqy_category` VALUES ('1931365d-21a5-4937-8df9-67624ce2805f', '修', '0', b'1');
INSERT INTO `zqy_category` VALUES ('2ef485e3-46be-4bbe-8ee6-57c790cdca07', '2323修改', '121218', b'0');
INSERT INTO `zqy_category` VALUES ('3cd443a6-7b81-4b64-a68c-19e0018e330d', '修改', '0', b'0');
INSERT INTO `zqy_category` VALUES ('430d995f-a416-45c4-be0e-d5851d8d6a1f', '母婴用品', '0', b'0');
INSERT INTO `zqy_category` VALUES ('48174564-47ef-4cf7-a225-e6f0a0e08f7f', '生鲜水果', '0', b'0');
INSERT INTO `zqy_category` VALUES ('50e8edc8-23e7-4012-8488-3e1c34a4fcaf', '12312修', '0', b'0');
INSERT INTO `zqy_category` VALUES ('631cbada-d36f-45f0-937f-0b898df79a10', '休闲娱乐', '121212', b'0');
INSERT INTO `zqy_category` VALUES ('7201e8d8-6014-493b-ac46-50e55634a323', '性感', '121218', b'0');
INSERT INTO `zqy_category` VALUES ('720954fe-45ee-4963-ac2a-428c2bc5bbeb', '23', '121218', b'0');
INSERT INTO `zqy_category` VALUES ('73041a0d-82f9-411c-8d2f-b5b742e7517a', '大家电', '0', b'0');
INSERT INTO `zqy_category` VALUES ('8f16ceb2-c162-4a69-8b60-5a84a154cb8f', '先日风情', '121212', b'0');
INSERT INTO `zqy_category` VALUES ('918a5b90-378b-432e-8ee5-5a8a39d64672', '修改类型', '0', b'0');
INSERT INTO `zqy_category` VALUES ('b32b1aff-53b9-4e92-808c-c6dd837ceb8e', '122323', '0', b'0');
INSERT INTO `zqy_category` VALUES ('b695697d-5273-415c-8bfd-faa118ee89a4', '新增类型', '121212', b'0');
INSERT INTO `zqy_category` VALUES ('b933c0ff-e41a-486b-8eaf-55c21e8eee98', '122323', '0', b'0');
INSERT INTO `zqy_category` VALUES ('bfacc454-a30b-47e2-8a7a-54a6de32a1df', '12五2323', '0', b'0');
INSERT INTO `zqy_category` VALUES ('c5eb400d-2f13-43ee-b2ba-93eaa043acc2', '12修改', '0', b'0');
INSERT INTO `zqy_category` VALUES ('cd6a7357-8093-46cb-871e-b10d3b90ab2a', '医疗保健', '0', b'0');
INSERT INTO `zqy_category` VALUES ('e52ba914-544e-4026-9ae1-bbf9135a0d21', '修订及', '0', b'0');
INSERT INTO `zqy_category` VALUES ('e8d29cb9-5855-4737-bc04-571921a63814', '疯狂一下', '121212', b'0');
INSERT INTO `zqy_category` VALUES ('f27d597f-a468-4369-ba85-659b5583bb4d', '优雅', '121213', b'0');

-- ----------------------------
-- Table structure for zqy_customer
-- ----------------------------
DROP TABLE IF EXISTS `zqy_customer`;
CREATE TABLE `zqy_customer`  (
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
-- Records of zqy_customer
-- ----------------------------
INSERT INTO `zqy_customer` VALUES ('5069b14e-c5ce-4ab4-97aa-5341d58f2aa9', '1', '1', '121', '121', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('5ea694e7-59b8-4d39-a3e7-f98387f3cbae', '@5ea694e7-59b8-4d39-a3e7-f98387f3cbae', '@UsersName', '@Nick', '@Pwd', '@Email', b'0');
INSERT INTO `zqy_customer` VALUES ('60fd5906-d36a-4fa4-b20a-4017b67d27e9', '地方', ' 地方 ', '对方的', '对方的', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('6659d23c-5b4b-4e69-b7a5-d432002d6b37', NULL, NULL, NULL, NULL, '?', b'0');
INSERT INTO `zqy_customer` VALUES ('73a37079-1bd9-4df8-890c-f45a48952253', '睡得', '是多少', '是的', '睡得是的', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('771ba104-1425-476e-9680-6bde60908624', '33', '23', '23', '23', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('81c6588f-491f-4ce7-ac22-f08813d1db28', '是', '是的', '是的', '是的', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('8bf952e0-5ff1-4b79-993a-615ac0f04a6a', '121', '12', '12', '12', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('957ef35b-ce93-40f4-bf9f-16f22ff01bd9', 'asa ', 'asa ', 'asa', 'asa', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('9a2341ae-bd9a-464b-9afd-2be67ab5c6ac', '是的', '是的', '为', '是的', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('@Email', '@UserId', '@UsersName', '@Nick', '@Pwd', '@Email', b'0');
INSERT INTO `zqy_customer` VALUES ('@UserId', 'dfd', '@Pwd', '@Email', '@Nick', '@DeliveryId', b'0');
INSERT INTO `zqy_customer` VALUES ('a3679902-f105-47cb-a879-5af5382d5a3d', '是的', '是的', '是的', '是的', '?', b'0');
INSERT INTO `zqy_customer` VALUES ('de6ead6f-7771-4760-87db-b6b4df955dfc', '是的', '是的', '是的', '是的', '?', b'0');

-- ----------------------------
-- Table structure for zqy_delivery
-- ----------------------------
DROP TABLE IF EXISTS `zqy_delivery`;
CREATE TABLE `zqy_delivery`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Consignee` int(0) NULL DEFAULT NULL,
  `Complete` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Phone` varchar(12) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_detail
-- ----------------------------
DROP TABLE IF EXISTS `zqy_detail`;
CREATE TABLE `zqy_detail`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OrdersID` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ProductId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Quantity` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `States` int(0) NULL DEFAULT NULL COMMENT '明细状态：0正常，1退货中，2已退货',
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_favorite
-- ----------------------------
DROP TABLE IF EXISTS `zqy_favorite`;
CREATE TABLE `zqy_favorite`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProductId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UserId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_files
-- ----------------------------
DROP TABLE IF EXISTS `zqy_files`;
CREATE TABLE `zqy_files`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProductId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Url` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  `IsIcon` bit(1) NULL DEFAULT NULL COMMENT '是否封面',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of zqy_files
-- ----------------------------
INSERT INTO `zqy_files` VALUES ('50d75b89-c435-4759-b15d-ab514af3191c', '8c1aa1a2-f572-4749-ba41-b4f4dd41ea00', '../../img/2019-08-05/4f8dfe17-4db3-4e51-bc70-38a8b16c1c8b.jpg', b'0', b'0');
INSERT INTO `zqy_files` VALUES ('8e668401-8022-4099-a76c-60c7009947bc', 'ea076d91-a091-409f-a2af-f4347bd9f4e0', '../../img/2019-08-02/39bdd744-e5ff-4cb7-979a-2e6b11bb139a.jpg', b'0', b'0');
INSERT INTO `zqy_files` VALUES ('fb12973a-fcff-4991-be26-d5536fabd946', 'b3e93f8e-adb9-4505-922c-b34f485e14d9', '../../img/2019-08-02/39bdd744-e5ff-4cb7-979a-2e6b11bb139a.jpg', b'0', b'0');

-- ----------------------------
-- Table structure for zqy_news
-- ----------------------------
DROP TABLE IF EXISTS `zqy_news`;
CREATE TABLE `zqy_news`  (
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
-- Records of zqy_news
-- ----------------------------
INSERT INTO `zqy_news` VALUES ('532b0584-e85a-4355-8451-8b055069d281', '商品名就', '23', '2323232323', NULL, '0001-01-01 00:00:00', '0', b'0');
INSERT INTO `zqy_news` VALUES ('bdb81c0a-65ce-4313-bdbb-f2759c4a5fca', '23', '231212', '23', NULL, '0001-01-01 00:00:00', '0', b'0');
INSERT INTO `zqy_news` VALUES ('bff3ea64-0f1b-4817-99e0-0e52c526ce2e', '23', '23', '23', '', '0001-01-01 00:00:00', '0', b'0');

-- ----------------------------
-- Table structure for zqy_orders
-- ----------------------------
DROP TABLE IF EXISTS `zqy_orders`;
CREATE TABLE `zqy_orders`  (
  `Id` int(0) NOT NULL,
  `Orderdate` datetime(0) NULL DEFAULT NULL,
  `UserId` int(0) NULL DEFAULT NULL,
  `Total` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DeliveryID` int(0) NULL DEFAULT NULL,
  `DeliveryDate` datetime(0) NULL DEFAULT NULL,
  `States` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '订单状态：\r\n0未付款，1已付款，2已发货，3已收货，4已评价',
  `Remark` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for zqy_product
-- ----------------------------
DROP TABLE IF EXISTS `zqy_product`;
CREATE TABLE `zqy_product`  (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CateId` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `MarketPrice` decimal(10, 2) NULL DEFAULT NULL,
  `Price` decimal(10, 2) NULL DEFAULT NULL,
  `Content` tinytext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `PostTime` datetime(0) NULL DEFAULT NULL,
  `Stock` int(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NULL DEFAULT NULL,
  `Icon` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '封面',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of zqy_product
-- ----------------------------
INSERT INTO `zqy_product` VALUES ('2843a18a-4585-4c57-b857-a95d701935cf', '23', '631cbada-d36f-45f0-937f-0b898df79a10', 23.00, 2323.00, '232', '2019-08-02 00:00:00', 23, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');
INSERT INTO `zqy_product` VALUES ('8c1aa1a2-f572-4749-ba41-b4f4dd41ea00', '1212', '631cbada-d36f-45f0-937f-0b898df79a10', 121.00, 121.00, '121212', '2019-08-05 00:00:00', 121, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');
INSERT INTO `zqy_product` VALUES ('acb3a7a8-7a73-4f9b-8125-995635c15186', '121', '8f16ceb2-c162-4a69-8b60-5a84a154cb8f', 12.00, 12.00, '12121', '2019-08-02 00:00:00', 12, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');
INSERT INTO `zqy_product` VALUES ('c6ed5e25-808c-429d-8521-3d65b2221b6a', '2323', '631cbada-d36f-45f0-937f-0b898df79a10', 2323.00, 22323.00, '32323223', '2019-08-02 00:00:00', 23, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');
INSERT INTO `zqy_product` VALUES ('ea076d91-a091-409f-a2af-f4347bd9f4e0', '男鞋', '631cbada-d36f-45f0-937f-0b898df79a10', 121.00, 121.00, '121212121212', '2019-08-02 00:00:00', 121, b'0', 'https://images2018.cnblogs.com/blog/1296231/201808/1296231-20180824135415724-84694516.png');

SET FOREIGN_KEY_CHECKS = 1;
