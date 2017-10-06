/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

INSERT INTO [dbo].[Countries]
    ([CountryCode],[CountryName],[Language])
VALUES
    ('USA', 'United States','en-us')
GO

INSERT INTO [dbo].[VenueTypes]
    ([VenueType],[VenueTypeName],[EventTypeName],[EventTypeShortName],[EventTypeShortNamePlural],[Language])
VALUES
    ('multipurpose','Multi-Purpose Venue','Event', 'Event','Events','en-us'),
    ('classicalmusic','Classical Music Venue','Classical Concert','Concert','Concerts','en-us'),
    ('jazz','Jazz Venue','Jazz Session','Session','Sessions','en-us'),
    ('judo','Judo Venue','Judo Tournament','Tournament','Tournaments','en-us'),
    ('soccer','Soccer Venue','Soccer Match', 'Match','Matches','en-us'),
    ('motorracing','Motor Racing Venue','Car Race', 'Race','Races','en-us'),
    ('dance', 'Dance Venue', 'Dance Performance', 'Performance', 'Performances','en-us'),
    ('blues', 'Blues Venue', 'Blues Session', 'Session','Sessions','en-us' ),
    ('rockmusic','Rock Music Venue','Rock Concert','Concert', 'Concerts','en-us'),
    ('opera','Opera Venue','Opera','Opera','Operas','en-us'); 
	
-- Venue Initialization
--
-- All existing venue data will be deleted (excluding reference tables)
--
-----------------------------------------------------------------

-- Delete all current data
DELETE FROM [dbo].[Tickets]
DELETE FROM [dbo].[TicketPurchases]
DELETE FROM [dbo].[Customers]
DELETE FROM [dbo].[EventSections]
DELETE FROM [dbo].[Events]
DELETE FROM [dbo].[Sections]
DELETE FROM [dbo].[Venues]

-- Ids pre-computed as md5 hash of UTF8 encoding of the normalized tenant name, converted to Int 
-- These are the id values that will be used by the client application and PowerShell scripts to 
-- retrieve tenant-specific data.   
DECLARE @ContosoId INT = 1976168774


-- Venue
INSERT INTO [dbo].[Venues]
   ([VenueId],[VenueName],[VenueType],[AdminEmail],[AdminPassword],[PostalCode],[CountryCode])
     VALUES
           (@ContosoId,'Contoso Concert Hall','classicalmusic','admin@contosoconcerthall.com',NULL,'98052','USA')

-- Sections
SET IDENTITY_INSERT [dbo].[Sections] ON
INSERT INTO [dbo].[Sections]
    ([VenueId],[SectionId],[SectionName],[SeatRows],[SeatsPerRow],[StandardPrice])
    VALUES
    (@ContosoId,1,'Main Auditorium Stage',10,30,100.00),
    (@ContosoId,2,'Main Auditorium Middle',10,30,80.00),
    (@ContosoId,3,'Main Auditorium Rear',10,30,60.00),
    (@ContosoId,4,'Balcony',10,30,40.00)
;
SET IDENTITY_INSERT [dbo].[Sections] OFF

-- Events
SET IDENTITY_INSERT [dbo].[Events] ON
INSERT INTO [dbo].[Events]
    ([VenueId],[EventId],[EventName],[Subtitle],[Date])
    VALUES
    (@ContosoId,1,'String Serenades','Contoso Chamber Orchestra','2017-02-10 20:00:00'),
    (@ContosoId,2,'Concert Pops', 'Contoso Symphony', '2017-02-11 20:00:00'),
    (@ContosoId,3,'A Musical Journey','Contoso Symphony','2017-02-12 20:00:00'),
    (@ContosoId,4,'A Night at the Opera','Contoso Choir', '2017-02-13 20:00:00'),
    (@ContosoId,5,'An Evening with Tchaikovsky','Contoso Symphony','2017-02-14 20:00:00'),
    (@ContosoId,6,'Lend me a Tenor','Contoso Choir','2017-02-15 20:00:00'),
    (@ContosoId,7,'Chamber Music Medley','Contoso Chamber Orchestra','2017-02-16 20:00:00'),
    (@ContosoId,8,'The 1812 Overture','Contoso Symphony','2017-02-17 20:00:00'),
    (@ContosoId,9,'Handel''s Messiah','Contoso Symphony', '2017-02-18 20:00:00'),
    (@ContosoId,10,'Moonlight Serenade','Contoso Quartet','2017-02-19 20:00:00'),
    (@ContosoId,11,'Seriously Strauss', 'Julie von Strauss Septet','2017-02-20 20:00:00') 
;
SET IDENTITY_INSERT [dbo].[Events] OFF

-- Event Sections
INSERT INTO [dbo].[EventSections]
    ([VenueId],[EventId],[SectionId],[Price])
    VALUES
    (@ContosoId,1,1,100.00),
    (@ContosoId,1,2,80.00),
    (@ContosoId,1,3,60.00),
    (@ContosoId,1,4,40.00),
    (@ContosoId,2,1,100.00),
    (@ContosoId,2,2,80.00),
    (@ContosoId,2,3,60.00),
    (@ContosoId,2,4,40.00),
    (@ContosoId,3,1,100.00),
    (@ContosoId,3,2,80.00),
    (@ContosoId,3,3,60.00),
    (@ContosoId,3,4,40.00),   
    (@ContosoId,4,1,100.00),
    (@ContosoId,4,2,80.00),
    (@ContosoId,4,3,60.00),
    (@ContosoId,4,4,40.00),
    (@ContosoId,5,1,100.00),
    (@ContosoId,5,2,80.00),
    (@ContosoId,5,3,60.00),
    (@ContosoId,5,4,40.00),
    (@ContosoId,6,1,100.00),
    (@ContosoId,6,2,80.00),
    (@ContosoId,6,3,60.00),
    (@ContosoId,6,4,40.00),
    (@ContosoId,7,1,100.00),
    (@ContosoId,7,2,80.00),
    (@ContosoId,7,3,60.00),
    (@ContosoId,7,4,40.00),
    (@ContosoId,8,1,100.00),
    (@ContosoId,8,2,80.00),
    (@ContosoId,8,3,60.00),
    (@ContosoId,8,4,40.00),
    (@ContosoId,9,1,100.00),
    (@ContosoId,9,2,80.00),
    (@ContosoId,9,3,60.00),
    (@ContosoId,9,4,40.00),
    (@ContosoId,10,1,100.00),
    (@ContosoId,10,2,80.00),
    (@ContosoId,10,3,60.00),
    (@ContosoId,10,4,40.00),
    (@ContosoId,11,1,150.00),
    (@ContosoId,11,2,100.00),
    (@ContosoId,11,3,90.00),
    (@ContosoId,11,4,60.00)     
GO
