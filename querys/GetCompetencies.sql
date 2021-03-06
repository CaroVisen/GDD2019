USE [GDD]
GO
/****** Object:  StoredProcedure [dbo].[GetCompetencies]    Script Date: 05/11/2009 22:47:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetCompetencies](@group numeric(18,0), @userid varchar(10), @status int)
as
declare @FullName varchar(200),
@Id varchar(50) ,
@Status varchar(50),
@Funcion varchar(50) ,
@Sql varchar(7990),
@Sql2 varchar(7990),
@Sql3 varchar(7990)

set @Sql = ''
set @Sql2 = ''
set @Sql3 = ''

set @status = case when @status = 1 then 0
                   when @status = 2 then 3
                   when @status = 3 then 6
                   when @status = 5 then 9
              end

   select b.FullName, 
		  Id = cast(a.Id as varchar(50)), 
		  [Status]= '_' + case when a.StatusId in(1,2,5,8,11) then 't' else 'f' end,
		  [StatusId] = a.StatusId,
		  b.Funcion,
		  c.EvaluatorComment,
		  c.ConcurrentComment,
		  c.DoubleReportComment,
          DoubleReport = case when a.DoubleReport is null then 'False' else 'True' end		
	 into #tmp 
     from NetworkAssessments a 
left join EmployeeFileRecordCache b  
	   on a.Evaluated = b.UserId
left join ResultComment c
	   on a.Id = c.NetworkAssessmentId
    where a.GroupId = @group
      and a.Evaluator = @userid
      and ((@status = 0) or a.StatusId = @status)
 order by b.Funcion, b.FullName


declare CurEvaluatedPeople cursor fast_forward
for (select FullName, Id, [Status], Funcion from #tmp)
open CurEvaluatedPeople
fetch next from CurEvaluatedPeople into @FullName, @Id, @Status, @Funcion
while (@@fetch_status <> -1)
begin
if (@@fetch_status <> -2)
	begin 
		set @Sql2 = @Sql2 + '''['+ @FullName +']_'+  @Id + @Status + '_' + @Funcion + ''' = IsNull(Max(Case When a.Id = ' + @Id + ' Then g.Result End),''0'')' +  ','
		fetch next from CurEvaluatedPeople into @FullName, @Id, @Status, @Funcion
	end
end
close CurEvaluatedPeople
deallocate CurEvaluatedPeople


set @Sql = '   select [CategoriId] = d.CategoryId,' + char(13) +
			'		  [CompetencyId] = f.Id,' + char(13) +
			'		  [Category] = e.Description,' + char(13) +
			'		  [Item] = f.Description,' + char(13)      
set @Sql2 = Left(@Sql2, len(@Sql2)-1)
set @Sql3 =	'      from NetworkAssessments a' + char(13) +
			' left join EmployeeFileRecordCache b'  + char(13) + 
			'		 on a.Evaluated = b.UserId' + char(13) +
			' left join [Group] c' + char(13) +
			'        on a.GroupId = c.Id' + char(13) +
			' left join CategoryByGroup d' + char(13) +
			'        on c.Id = d.GroupId' + char(13) +
			' left join Category e' + char(13) +
			'        on d.CategoryId = e.Id' + char(13) +
			' left join Competency f' + char(13) +
			'        on e.Id = f.CategoryId' + char(13) +
			' left join ResultCompetency g' + char(13) +
			'        on a.Id = g.NetworkAssessmentId ' + char(13) +
			'       and e.Id = g.CategoryId' + char(13) +
			'       and f.Id = g.CompetencyId' + char(13) +
			'     where a.GroupId = ' + cast(@group as varchar(10)) +  + char(13) +
			'	    and a.Evaluator = ''' + @userid + '''' + char(13) +
			'  group by d.CategoryId,' + char(13) +
			'           f.Id,' + char(13) +
			'           e.Description,' + char(13) +
			'		    f.Description,' + char(13) +
			' 	        d.[Order],'+ char(13) +
			'		    f.[Order]'+ char(13) +
			'  order by d.CategoryId,'+ char(13) +
			'           d.[Order],'+ char(13) +
			'		    f.[Order]'+ char(13) 

exec(@Sql + @Sql2 + @Sql3)

------------
--Comments -
------------ 
    select a.Id, 
           [Result] = sum(case when (b.BlockId = 1 and a.GroupId = 7) then b.Value/2 else b.Value end)
	  into #tmpResult	
      from NetworkAssessments a
 left join OverallResult b
        on b.NetworkAssessmentId = a.Id
     where a.GroupId = @group
       and a.Evaluator = @userid  
  group by a.Id
  order by 1

   select a.Id,
		  [FullName] = a.FullName + 
                       ' (' + a.Funcion + ') ' + isnull(+ 
					   case when a.StatusId not in(1,2,5,8,11) then 
							case when (b.Result >= 0 and b.Result <= 0.7999) then '- Resultado: 1'
								 when (b.Result >= 0.8000 and b.Result <= 0.8999) then '- Resultado: 2'
								 when (b.Result >= 0.9000 and b.Result <= 0.9499) then '- Resultado: 3'
								 when (b.Result >= 0.9500 and b.Result <= 0.9999) then '- Resultado: 4'
								 when (b.Result >= 1) then '- Resultado: 5'
							 end
						else
						 ''
						end, ''),
		  a.EvaluatorComment,
		  a.ConcurrentComment,
		  a.DoubleReportComment,
		  a.StatusId,
		  a.DoubleReport 	
     from #tmp a
left join #tmpResult b
       on a.Id = b.Id
 order by a.Funcion, 
		  a.FullName

--GetCompetencies 4, "AR01016114", 0
--GetCompetencies 4, "AR00025064"
--GetCompetencies 7, "AR00023966"

----delete from ResultCompetency --select * from Status
--select Evaluator,  count(*) from NetworkAssessments group by Evaluator order by 2 desc
-- Select * from NetworkAssessments where DoubleReport is not null
