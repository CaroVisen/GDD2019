USE [GDD]
GO
/****** Object:  StoredProcedure [dbo].[GetManagementIndicators]    Script Date: 05/11/2009 22:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetManagementIndicators](@group numeric(18,0), @userid varchar(10), @status int)
as
declare @state int
set @status = case when @status = 1 then 0
                   when @status = 2 then 3
                   when @status = 3 then 6
                   when @status = 5 then 9
              end

	select Id,
		   Evaluated,
		   Evaluator,
		   Concurrent,
		   DoubleReport,
		   GroupId,
		   VariableId,
		   StatusId
      into #tmpNetworkAssessments
	  from NetworkAssessments
     where GroupId = @group
       and Evaluator = @userid
       and ((@status = 0) or StatusId = @status)

    select FullName = b.FullName + ' (' + b.Funcion + ')', 
		   [Description] = case when d.Code = 0 then e.Description else d.Description end,
		   Weight = case when d.Code = 0 then isnull(e.Weight, 0) else isnull(d.Weight,0) end,		
		   Unit = '% Cump.',
		   [Result]= isnull(e.Result, 0),
		   [Group] =c.Description,
		   [Status]= case when (a.StatusId in(1,2,5,8,11)) then 'true' else 'false' end,
		   Code = d.Code,
		   NetworkAssessmentsId= a.Id,
		   VariableId= d.Id,	
		   Letter=f.Letter + '_' + cast(f.Value as varchar(50))			
      from #tmpNetworkAssessments a
 left join EmployeeFileRecordCache b  
		on a.Evaluated = b.UserId
 left join [Group] c
        on a.GroupId = c.Id
 left join Variable d
        on a.VariableId = d.Code
 left join ResultVariable e
        on a.Id = e.NetworkAssessmentId 
       and d.Id = e.VariableId
 left join OverallResult f
        on a.Id = f.NetworkAssessmentId
       and f.BlockId = 1 
    where a.GroupId = @group
	   and a.Evaluator = @userid
	   and ((a.StatusId in(1,2,5,8,11) 
             or (e.Description is not null) 
             or (d.Description is not null)))
  order by c.Description,
           b.Funcion, 
           b.FullName,
           d.[Order]


--GetManagementIndicators 4, 'AR01016114', 4
--GetManagementIndicators 7, 'AR00023500', 0
--update NetworkAssessments set StatusId = 3
