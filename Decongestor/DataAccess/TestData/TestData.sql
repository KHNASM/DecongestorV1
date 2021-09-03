USE [Decongestor_Dev]
GO
SET IDENTITY_INSERT [dbo].[VehicleTypes] ON 
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (1, N'Car', NULL)
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (2, N'Bus', NULL)
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (3, N'Lorry', NULL)
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (4, N'Van', NULL)
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (5, N'Motorbike', CAST(0.00 AS Decimal(5, 2)))
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (6, N'Tractor', CAST(0.00 AS Decimal(5, 2)))
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (7, N'Emergency', CAST(0.00 AS Decimal(5, 2)))
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (8, N'Diplomat', CAST(0.00 AS Decimal(5, 2)))
GO
INSERT [dbo].[VehicleTypes] ([Id], [Description], [DailyChargeCap]) VALUES (9, N'Military', CAST(0.00 AS Decimal(5, 2)))
GO
SET IDENTITY_INSERT [dbo].[VehicleTypes] OFF
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'ABCE765', N'Janes Tractor', 6)
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'AJK2440', N'Johns Lorry', 3)
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'AJKD7172', N'KI Ambulance', 7)
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'AJKF1940', N'Asim''s Bike', 5)
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'ASED1287', N'Millitary Transport Truck', 9)
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'FERD8847', N'Uber Van', 4)
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'GRDT2281', N'Private Car', 1)
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'HHDD882', N'US Embassy Vehicle', 8)
GO
INSERT [dbo].[Vehicles] ([Id], [Description], [VehicleTypeId]) VALUES (N'PBTR111', N'Malmo FC Bus', 2)
GO
