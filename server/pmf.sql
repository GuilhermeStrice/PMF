-- phpMyAdmin SQL Dump
-- version 4.8.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: 18-Abr-2020 às 04:08
-- Versão do servidor: 10.1.33-MariaDB
-- PHP Version: 7.2.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `pmf`
--

-- --------------------------------------------------------

--
-- Estrutura da tabela `asset_table`
--

CREATE TABLE `asset_table` (
  `id_asset` int(11) NOT NULL,
  `id_package` int(11) NOT NULL,
  `Version` varchar(50) NOT NULL,
  `SdkVersion` varchar(50) NOT NULL,
  `Checksum` varchar(150) NOT NULL,
  `FileName` varchar(150) NOT NULL,
  `Url` varchar(2500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `asset_table`
--

INSERT INTO `asset_table` (`id_asset`, `id_package`, `Version`, `SdkVersion`, `Checksum`, `FileName`, `Url`) VALUES
(1, 1, '0.0.1', '0.0.5', 'asdfasdfasdf', 'asdf.zip', 'http://localhost:3000/asdfasdf.zip');

-- --------------------------------------------------------

--
-- Estrutura da tabela `dependency`
--

CREATE TABLE `dependency` (
  `id_dependency` int(11) NOT NULL,
  `id_asset` int(11) NOT NULL,
  `ID` varchar(50) NOT NULL,
  `Checksum` varchar(150) NOT NULL,
  `FileName` varchar(150) NOT NULL,
  `Url` varchar(2500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `dependency`
--

INSERT INTO `dependency` (`id_dependency`, `id_asset`, `ID`, `Checksum`, `FileName`, `Url`) VALUES
(1, 1, 'stuff', 'asdfasdf', 'asdfasdf', 'http://localhost:3000/asdfasdf.zip');

-- --------------------------------------------------------

--
-- Estrutura da tabela `package`
--

CREATE TABLE `package` (
  `id_package` int(11) NOT NULL,
  `ID` varchar(50) NOT NULL,
  `Type` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Author` varchar(50) NOT NULL,
  `Description` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `package`
--

INSERT INTO `package` (`id_package`, `ID`, `Type`, `Name`, `Author`, `Description`) VALUES
(1, 'test', 1, 'Test Package', '', 'This is a package for testing stuff and things');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `asset_table`
--
ALTER TABLE `asset_table`
  ADD PRIMARY KEY (`id_asset`),
  ADD KEY `id_package` (`id_package`);

--
-- Indexes for table `dependency`
--
ALTER TABLE `dependency`
  ADD PRIMARY KEY (`id_dependency`),
  ADD KEY `id_asset` (`id_asset`);

--
-- Indexes for table `package`
--
ALTER TABLE `package`
  ADD PRIMARY KEY (`id_package`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `asset_table`
--
ALTER TABLE `asset_table`
  MODIFY `id_asset` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `dependency`
--
ALTER TABLE `dependency`
  MODIFY `id_dependency` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `package`
--
ALTER TABLE `package`
  MODIFY `id_package` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Constraints for dumped tables
--

--
-- Limitadores para a tabela `asset_table`
--
ALTER TABLE `asset_table`
  ADD CONSTRAINT `FK_asset_table_package` FOREIGN KEY (`id_package`) REFERENCES `package` (`id_package`);

--
-- Limitadores para a tabela `dependency`
--
ALTER TABLE `dependency`
  ADD CONSTRAINT `FK_dependency_asset_table` FOREIGN KEY (`id_asset`) REFERENCES `asset_table` (`id_asset`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
