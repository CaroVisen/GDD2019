USE [GDD]
GO
/****** Object:  StoredProcedure [dbo].[GetEvaluatedPeople]    Script Date: 05/11/2009 20:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[GetEvaluatedPeople](@userid varchar(10))
as
    select a.Id, 
           [Result] = sum(case when (b.BlockId = 1 and a.GroupId = 7) then b.Value/2 else b.Value end)
	  into #tmpResult	
      from NetworkAssessments a
 left join OverallResult b
        on b.NetworkAssessmentId = a.Id
  group by a.Id
  order by 1

    select Evaluator, 
		   [Status] = min(StatusId)
	  into #tmpStatus
      from NetworkAssessments
  group by Evaluator
  order by 1

----------------------------------
-- Evaluator
----------------------------------
    select [Group] = c.Description, 
           b.FullName, 
           b.Funcion, 
           Result =case when a.StatusId not in(1,2,5,8,11) then 
						case when (e.Result >= 0 and e.Result <= 0.7999) then 'A'
							 when (e.Result >= 0.8000 and e.Result <= 0.8999) then 'B'
							 when (e.Result >= 0.9000 and e.Result <= 0.9499) then 'C'
							 when (e.Result >= 0.9500 and e.Result <= 0.9999) then 'C+'
							 when (e.Result >= 1) then 'D'
						end
					else
						'-'
					end,
           [Status] = d.Description,
		   [Color] = case when a.StatusId =1 then 'tdR'
						  when a.StatusId =12 then 'tdG'
					 else 'tdY'
					 end,
		   GroupId = c.Id,
		   StatusId = d.Id,
		   [Action] = isnull(f.Status, 0),
		   a.Id						 
      from NetworkAssessments a
 left join EmployeeFileRecordCache b  
		on a.Evaluated = b.UserId
 left join [Group] c
        on a.GroupId = c.Id
 left join [Status] d
        on a.StatusId = d.Id 
 left join #tmpResult e
        on a.Id = e.Id
 left join #tmpStatus f
        on a.Evaluator = f.Evaluator
     where a.Evaluator = @userid
  order by 1,3,2

----------------------------------
-- Concurrent
----------------------------------
    select c.FullName,
		   [Group] = d.Description, 
		   b.FullName, 
           b.Funcion, 
           Result = case when (f.Result >= 0 and f.Result <= 0.7999) then 'A'
						 when (f.Result >= 0.8000 and f.Result <= 0.8999) then 'B'
						 when (f.Result >= 0.9000 and f.Result <= 0.9499) then 'C'
						 when (f.Result >= 0.9500 and f.Result <= 0.9999) then 'C+'
						 when (f.Result >= 1) then 'D'
					else '-'
                    end, 
           [Status] = e.Description,
		   [Color] = case when a.StatusId =1 then 'tdR'
						  when a.StatusId =12 then 'tdG'
					 else 'tdY'
					 end,
		   GroupId = d.Id,
		   StatusId = e.Id,
		   a.Evaluator							 
      from NetworkAssessments a
 left join EmployeeFileRecordCache b  
		on a.Evaluated = b.UserId
 left join EmployeeFileRecordCache c
		on a.Evaluator = c.UserId
 left join [Group] d
        on a.GroupId = d.Id
 left join [Status] e
        on a.StatusId = e.Id 
 left join #tmpResult f
        on a.Id = f.Id	
     where a.Concurrent = @userid
       and a.StatusId = 3
  order by 1,2,4,3


----------------------------------
-- Double Report
----------------------------------
    select c.FullName,
		   [Group] = d.Description, 
		   b.FullName, 
           b.Funcion, 
           Result = case when (f.Result >= 0 and f.Result <= 0.7999) then 'A'
						 when (f.Result >= 0.8000 and f.Result <= 0.8999) then 'B'
						 when (f.Result >= 0.9000 and f.Result <= 0.9499) then 'C'
						 when (f.Result >= 0.9500 and f.Result <= 0.9999) then 'C+'
						 when (f.Result >= 1) then 'D'
					else '-'
                    end, 
           [Status] = e.Description,
		   [Color] = case when a.StatusId =1 then 'tdR'
						  when a.StatusId =12 then 'tdG'
					 else 'tdY'
					 end,
		   GroupId = d.Id,
		   StatusId = e.Id,
		   a.Evaluator							 
      from NetworkAssessments a
 left join EmployeeFileRecordCache b  
		on a.Evaluated = b.UserId
 left join EmployeeFileRecordCache c
		on a.Evaluator = c.UserId
 left join [Group] d
        on a.GroupId = d.Id
 left join [Status] e
        on a.StatusId = e.Id 
 left join #tmpResult f
        on a.Id = f.Id	
     where a.DoubleReport = @userid
       and a.StatusId = 6
  order by 1,2,4,3

----------------------------------
-- RRHH
----------------------------------
    select c.FullName,
		   [Group] = d.Description, 
		   b.FullName, 
           b.Funcion, 
           Result = case when (f.Result >= 0 and f.Result <= 0.7999) then 'A'
						 when (f.Result >= 0.8000 and f.Result <= 0.8999) then 'B'
						 when (f.Result >= 0.9000 and f.Result <= 0.9499) then 'C'
						 when (f.Result >= 0.9500 and f.Result <= 0.9999) then 'C+'
						 when (f.Result >= 1) then 'D'
					else '-'
                    end, 
           [Status] = e.Description,
		   [Color] = case when a.StatusId =1 then 'tdR'
						  when a.StatusId =12 then 'tdG'
					 else 'tdY'
					 end,
		   GroupId = d.Id,
		   StatusId = e.Id,
		   a.Evaluator 
      from NetworkAssessments a
 left join EmployeeFileRecordCache b  
		on a.Evaluated = b.UserId
 left join EmployeeFileRecordCache c
		on a.Evaluator = c.UserId
 left join [Group] d
        on a.GroupId = d.Id
 left join [Status] e
        on a.StatusId = e.Id 
 left join #tmpResult f
        on a.Id = f.Id
 left join UserAccessControl g
        on g.UserId = @userid
     where a.StatusId in(9)
       and g.ProfileId in (4,5)
	   and ((g.Locals is null)
             or (charindex(b.Local + '|', g.Locals ) > 0)
           )	
       --and c.FullName is not null -- delete this line before implementation
  order by 1,2,4,3

--GetEvaluatedPeople 'AR01016114'
--GetEvaluatedPeople 'AR00023966'
--select * from EmployeeFileRecordCache where UserId ='AR00026244'
--update NetworkAssessments set StatusId = 312
--select Local from EmployeeFileRecordCache group by Local
--Reset 1177
--ROCA/SERVET|
--PLANTA 4|
--PARRAL|
--ADM.CENTRAL|
--LOMA HERMOSA|
--PLANTA ALCORTA|
--MATANZA|
--SAN JUSTO|
--PILAR|
--select * from NetworkAssessments where Evaluator ='AR01016114'
