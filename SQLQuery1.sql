--CREATE PROCEDURE [dbo].[b2_monthly_emp_prod_sheet_sp]
--	@unit  int,
--	@department int,
--	@section int,
--	@block int,
--	@empId int,
--	@fromdate date,
--	@todate date
--AS

	declare @unit  int
	declare  @department int
	declare @section int
	declare @block int
	declare @empId int
	declare  @fromdate date
	declare @todate date

	set @unit  = 1
	set  @department = 1
	set @section = 5
	set @block = 1
	set @empId = null
	set  @fromdate = '2022-08-21'
	set @todate = '2022-11-20'

begin
	SELECT        b2_department_info.department, si.section,b2_block_info.block, eb.emp_cardno, eb.emp_name, 
							b2_designation_info.designation, STRING_AGG(Day(fp.prod_date),',') as Prod_Date,
							sii.style, pii.process_name, sum(fp.quantity) as Quantity, fp.process_by, 						
							b2_company_info.company_name,b2_building_info.building_name
	FROM            b2_floor_production_list AS fp INNER JOIN
							 b2_emp_basic AS eb ON eb.emp_id = fp.emp_id INNER JOIN
							 b2_section_info AS si ON si.section_id = fp.section INNER JOIN
							 b2_style_info AS sii ON sii.style_id = fp.style INNER JOIN
							 b2_process_info AS pii ON pii.process_id = fp.process INNER JOIN
							 b2_department_info ON eb.department = b2_department_info.deptId AND si.department = b2_department_info.deptId INNER JOIN
							 b2_designation_info ON eb.designation = b2_designation_info.desigId INNER JOIN
							 b2_block_info ON eb.block = b2_block_info.blockId INNER JOIN
							 b2_building_info ON eb.unit = b2_building_info.building_id INNER JOIN
							 b2_shift_info ON eb.shift = b2_shift_info.shiftId INNER JOIN
							 b2_company_info ON b2_building_info.company = b2_company_info.company_id
	where b2_building_info.building_id = @unit or @unit is null
	and b2_department_info.deptId = @department or @department is null
	and fp.section = @section or @section is null
	and eb.block = @block or @block is null
	and fp.emp_id = @empId or @empId is null
	and fp.prod_date between CONVERT(date,@fromdate) and CONVERT(date,@todate)
	group by si.section, eb.emp_cardno, eb.emp_name,sii.style, pii.process_name, fp.process_by, b2_department_info.department, b2_block_info.block, b2_designation_info.designation, b2_building_info.building_name, b2_company_info.company_name
end
