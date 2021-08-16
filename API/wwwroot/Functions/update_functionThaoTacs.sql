DELETE Function_ThaoTacs;

INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
select NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
from Roles r
cross join Functions f
cross join ThaoTacs tt
where (f.Type = N'Hóa đơn' and tt.Ma Like N'HD%')
or (f.Type = N'Hệ thống' and tt.Ma like N'SYS%')
or (f.Type = N'Báo cáo' and tt.Ma like N'BC%')
or (f.Type = N'Tiện ích' and tt.Ma like N'TOOL%')
or (f.Type = N'Danh mục' and tt.Ma Like N'DM%')
