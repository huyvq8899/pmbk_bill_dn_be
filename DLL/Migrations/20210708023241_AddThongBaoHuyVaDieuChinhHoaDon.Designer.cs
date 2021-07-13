﻿// <auto-generated />
using System;
using DLL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DLL.Migrations
{
    [DbContext(typeof(Datacontext))]
    [Migration("20210708023241_AddThongBaoHuyVaDieuChinhHoaDon")]
    partial class AddThongBaoHuyVaDieuChinhHoaDon
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DLL.Entity.Config.TuyChon", b =>
                {
                    b.Property<string>("Ma")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GiaTri");

                    b.Property<string>("Ten");

                    b.HasKey("Ma");

                    b.ToTable("TuyChons");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.DoiTuong", b =>
                {
                    b.Property<string>("DoiTuongId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChiNhanh");

                    b.Property<string>("ChucDanh");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DiaChi");

                    b.Property<string>("EmailNguoiMuaHang");

                    b.Property<string>("EmailNguoiNhanHD");

                    b.Property<string>("HoTenNguoiMuaHang");

                    b.Property<string>("HoTenNguoiNhanHD");

                    b.Property<bool?>("IsKhachHang");

                    b.Property<bool?>("IsNhanVien");

                    b.Property<int?>("LoaiKhachHang");

                    b.Property<string>("Ma");

                    b.Property<string>("MaSoThue");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<int?>("STT");

                    b.Property<string>("SoDienThoaiNguoiMuaHang");

                    b.Property<string>("SoDienThoaiNguoiNhanHD");

                    b.Property<string>("SoTaiKhoanNganHang");

                    b.Property<bool>("Status");

                    b.Property<string>("Ten");

                    b.Property<string>("TenDonVi");

                    b.Property<string>("TenNganHang");

                    b.HasKey("DoiTuongId");

                    b.ToTable("DoiTuongs");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.DonViTinh", b =>
                {
                    b.Property<string>("DonViTinhId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("MoTa");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<int?>("STT");

                    b.Property<bool>("Status");

                    b.Property<string>("Ten");

                    b.HasKey("DonViTinhId");

                    b.ToTable("DonViTinhs");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.HangHoaDichVu", b =>
                {
                    b.Property<string>("HangHoaDichVuId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<decimal?>("DonGiaBan");

                    b.Property<string>("DonViTinhId");

                    b.Property<bool?>("IsGiaBanLaDonGiaSauThue");

                    b.Property<string>("Ma");

                    b.Property<string>("MoTa");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<int?>("STT");

                    b.Property<bool>("Status");

                    b.Property<string>("Ten");

                    b.Property<int>("ThueGTGT");

                    b.Property<decimal?>("TyLeChietKhau");

                    b.HasKey("HangHoaDichVuId");

                    b.HasIndex("DonViTinhId");

                    b.ToTable("HangHoaDichVus");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.HoSoHDDT", b =>
                {
                    b.Property<string>("HoSoHDDTId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChiNhanh");

                    b.Property<string>("CoQuanThueCapCuc");

                    b.Property<string>("CoQuanThueQuanLy");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DiaChi");

                    b.Property<string>("EmailLienHe");

                    b.Property<string>("EmailNguoiDaiDienPhapLuat");

                    b.Property<string>("Fax");

                    b.Property<string>("HoTenNguoiDaiDienPhapLuat");

                    b.Property<string>("MaSoThue");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("NganhNgheKinhDoanhChinh");

                    b.Property<int?>("STT");

                    b.Property<string>("SoDienThoaiLienHe");

                    b.Property<string>("SoDienThoaiNguoiDaiDienPhapLuat");

                    b.Property<string>("SoTaiKhoanNganHang");

                    b.Property<bool>("Status");

                    b.Property<string>("TenDonVi");

                    b.Property<string>("TenNganHang");

                    b.Property<string>("Website");

                    b.HasKey("HoSoHDDTId");

                    b.ToTable("HoSoHDDTs");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.LoaiTien", b =>
                {
                    b.Property<string>("LoaiTienId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Ma");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<int?>("STT");

                    b.Property<int?>("SapXep");

                    b.Property<bool>("Status");

                    b.Property<string>("Ten");

                    b.Property<decimal?>("TyGiaQuyDoi");

                    b.HasKey("LoaiTienId");

                    b.ToTable("LoaiTiens");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.MauHoaDon", b =>
                {
                    b.Property<string>("MauHoaDonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("KyHieu");

                    b.Property<int>("LoaiHoaDon");

                    b.Property<int>("LoaiKhoGiay");

                    b.Property<int>("LoaiMauHoaDon");

                    b.Property<int>("LoaiNgonNgu");

                    b.Property<int>("LoaiThueGTGT");

                    b.Property<string>("MauSo");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<int>("QuyDinhApDung");

                    b.Property<int?>("STT");

                    b.Property<int?>("SoThuTu");

                    b.Property<bool>("Status");

                    b.Property<string>("Ten");

                    b.Property<string>("TenBoMau");

                    b.HasKey("MauHoaDonId");

                    b.ToTable("MauHoaDons");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.MauHoaDonThietLapMacDinh", b =>
                {
                    b.Property<string>("MauHoaDonThietLapMacDinhId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("GiaTri");

                    b.Property<string>("Height");

                    b.Property<string>("Left");

                    b.Property<int>("Loai");

                    b.Property<string>("MauHoaDonId");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Opacity");

                    b.Property<int?>("STT");

                    b.Property<bool>("Status");

                    b.Property<string>("Top");

                    b.Property<string>("Width");

                    b.HasKey("MauHoaDonThietLapMacDinhId");

                    b.HasIndex("MauHoaDonId")
                        .IsUnique()
                        .HasFilter("[MauHoaDonId] IS NOT NULL");

                    b.ToTable("MauHoaDonThietLapMacDinhs");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoDieuChinhThongTinHoaDon", b =>
                {
                    b.Property<string>("ThongBaoDieuChinhThongTinHoaDonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoQuanThue");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DiaChiCu");

                    b.Property<string>("DiaChiMoi");

                    b.Property<string>("DienThoaiCu");

                    b.Property<string>("DienThoaiMoi");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<DateTime?>("NgayThongBaoDieuChinh");

                    b.Property<DateTime?>("NgayThongBaoPhatHanh");

                    b.Property<int?>("STT");

                    b.Property<string>("So");

                    b.Property<bool>("Status");

                    b.Property<string>("TaiLieuDinhKem");

                    b.Property<string>("TenDonViCu");

                    b.Property<string>("TenDonViMoi");

                    b.Property<int>("TrangThaiNop");

                    b.HasKey("ThongBaoDieuChinhThongTinHoaDonId");

                    b.ToTable("ThongBaoDieuChinhThongTinHoaDons");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoDieuChinhThongTinHoaDonChiTiet", b =>
                {
                    b.Property<string>("ThongBaoDieuChinhThongTinHoaDonId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("MauHoaDonId");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<int?>("STT");

                    b.Property<bool>("Status");

                    b.Property<string>("ThongBaoDieuChinhThongTinHoaDonChiTietId");

                    b.HasKey("ThongBaoDieuChinhThongTinHoaDonId");

                    b.HasIndex("MauHoaDonId");

                    b.ToTable("ThongBaoDieuChinhThongTinHoaDonChiTiets");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoKetQuaHuyHoaDon", b =>
                {
                    b.Property<string>("ThongBaoKetQuaHuyHoaDonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoQuanThue");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<DateTime?>("NgayGioHuy");

                    b.Property<DateTime?>("NgayThongBao");

                    b.Property<string>("PhuongPhapHuy");

                    b.Property<int?>("STT");

                    b.Property<bool>("Status");

                    b.Property<string>("TaiLieuDinhKem");

                    b.Property<int>("TrangThaiNop");

                    b.HasKey("ThongBaoKetQuaHuyHoaDonId");

                    b.ToTable("ThongBaoKetQuaHuyHoaDons");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoKetQuaHuyHoaDonChiTiet", b =>
                {
                    b.Property<string>("ThongBaoKetQuaHuyHoaDonChiTietId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<int?>("DenSo");

                    b.Property<string>("KyHieu");

                    b.Property<int>("LoaiHoaDon");

                    b.Property<string>("MauHoaDonId");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<int?>("STT");

                    b.Property<int?>("SoLuong");

                    b.Property<bool>("Status");

                    b.Property<string>("ThongBaoKetQuaHuyHoaDonId");

                    b.Property<int?>("TuSo");

                    b.HasKey("ThongBaoKetQuaHuyHoaDonChiTietId");

                    b.HasIndex("MauHoaDonId");

                    b.HasIndex("ThongBaoKetQuaHuyHoaDonId");

                    b.ToTable("ThongBaoKetQuaHuyHoaDonChiTiets");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoPhatHanh", b =>
                {
                    b.Property<string>("ThongBaoPhatHanhId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoQuanThue");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DienThoai");

                    b.Property<bool?>("IsXacNhan");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<DateTime>("Ngay");

                    b.Property<string>("NguoiDaiDienPhapLuat");

                    b.Property<int?>("STT");

                    b.Property<string>("So");

                    b.Property<bool>("Status");

                    b.Property<int>("TrangThaiNop");

                    b.HasKey("ThongBaoPhatHanhId");

                    b.ToTable("ThongBaoPhatHanhs");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoPhatHanhChiTiet", b =>
                {
                    b.Property<string>("ThongBaoPhatHanhChiTietId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<int?>("DenSo");

                    b.Property<string>("KyHieu");

                    b.Property<string>("MauHoaDonId");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<DateTime?>("NgayBatDauSuDung");

                    b.Property<int?>("STT");

                    b.Property<int?>("SoLuong");

                    b.Property<bool>("Status");

                    b.Property<string>("ThongBaoPhatHanhId");

                    b.Property<int?>("TuSo");

                    b.HasKey("ThongBaoPhatHanhChiTietId");

                    b.HasIndex("MauHoaDonId");

                    b.HasIndex("ThongBaoPhatHanhId");

                    b.ToTable("ThongBaoPhatHanhChiTiets");
                });

            modelBuilder.Entity("DLL.Entity.Function", b =>
                {
                    b.Property<string>("FunctionId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("FunctionName");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<int?>("STT");

                    b.Property<bool>("Status");

                    b.Property<string>("SubTitle");

                    b.Property<string>("Title");

                    b.Property<string>("Type");

                    b.HasKey("FunctionId");

                    b.ToTable("Functions");
                });

            modelBuilder.Entity("DLL.Entity.Function_Role", b =>
                {
                    b.Property<string>("FRID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<bool>("Active");

                    b.Property<string>("FunctionId");

                    b.Property<string>("PermissionId");

                    b.Property<string>("RoleId");

                    b.HasKey("FRID");

                    b.HasIndex("FunctionId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("Function_Roles");
                });

            modelBuilder.Entity("DLL.Entity.Function_ThaoTac", b =>
                {
                    b.Property<string>("FTID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("FunctionId");

                    b.Property<string>("PermissionId");

                    b.Property<string>("RoleId");

                    b.Property<string>("ThaoTacId");

                    b.HasKey("FTID");

                    b.HasIndex("FunctionId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("ThaoTacId");

                    b.ToTable("Function_ThaoTacs");
                });

            modelBuilder.Entity("DLL.Entity.Function_User", b =>
                {
                    b.Property<string>("FUID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("FunctionId");

                    b.Property<string>("PermissionId");

                    b.Property<string>("UserId");

                    b.HasKey("FUID");

                    b.HasIndex("FunctionId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId");

                    b.ToTable("Function_Users");
                });

            modelBuilder.Entity("DLL.Entity.KyKeToan", b =>
                {
                    b.Property<string>("KyKeToanId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool?>("DaKhoaNhapSoDuBanDau");

                    b.Property<bool?>("DaKhoaSo");

                    b.Property<DateTime?>("DenNgay");

                    b.Property<string>("GhiChu");

                    b.Property<int?>("LoaiThongTu");

                    b.Property<string>("ModifyBy");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<DateTime?>("NgayChungTu");

                    b.Property<int?>("STT");

                    b.Property<bool>("Status");

                    b.HasKey("KyKeToanId");

                    b.ToTable("KyKeToans");
                });

            modelBuilder.Entity("DLL.Entity.Permission", b =>
                {
                    b.Property<string>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("PermissionName");

                    b.Property<bool?>("Status");

                    b.HasKey("PermissionId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("DLL.Entity.Role", b =>
                {
                    b.Property<string>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("RoleName")
                        .HasMaxLength(200);

                    b.Property<bool>("Status");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DLL.Entity.ThaoTac", b =>
                {
                    b.Property<string>("ThaoTacId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ma");

                    b.Property<int>("STT");

                    b.Property<string>("Ten");

                    b.HasKey("ThaoTacId");

                    b.ToTable("ThaoTacs");
                });

            modelBuilder.Entity("DLL.Entity.User", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("Address");

                    b.Property<string>("Avatar");

                    b.Property<string>("ConfirmPassword")
                        .HasMaxLength(200);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(200);

                    b.Property<string>("FullName")
                        .HasMaxLength(200);

                    b.Property<int?>("Gender");

                    b.Property<bool?>("IsAdmin");

                    b.Property<bool?>("IsNodeAdmin");

                    b.Property<bool?>("IsOnline");

                    b.Property<int?>("LoginCount");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Password")
                        .HasMaxLength(200);

                    b.Property<string>("Phone")
                        .HasMaxLength(200);

                    b.Property<string>("RoleId");

                    b.Property<bool>("Status");

                    b.Property<string>("Title")
                        .HasMaxLength(200);

                    b.Property<string>("UserName")
                        .HasMaxLength(200);

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DLL.Entity.User_Role", b =>
                {
                    b.Property<string>("URID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("RoleId");

                    b.Property<string>("UserId");

                    b.HasKey("URID");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("User_Roles");
                });

            modelBuilder.Entity("DLL.Entity.ViewThaoTac", b =>
                {
                    b.Property<string>("ThaoTacId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("Active");

                    b.Property<string>("FTID");

                    b.Property<string>("FunctionId");

                    b.Property<string>("Ma");

                    b.Property<string>("PemissionId");

                    b.Property<string>("RoleId");

                    b.Property<int>("STT");

                    b.Property<string>("Ten");

                    b.Property<string>("UTID");

                    b.Property<string>("UserId");

                    b.HasKey("ThaoTacId");

                    b.ToTable("ViewThaoTacs");
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.HangHoaDichVu", b =>
                {
                    b.HasOne("DLL.Entity.DanhMuc.DonViTinh", "DonViTinh")
                        .WithMany("HangHoaDichVus")
                        .HasForeignKey("DonViTinhId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.MauHoaDonThietLapMacDinh", b =>
                {
                    b.HasOne("DLL.Entity.DanhMuc.MauHoaDon", "MauHoaDon")
                        .WithOne("MauHoaDonThietLapMacDinh")
                        .HasForeignKey("DLL.Entity.DanhMuc.MauHoaDonThietLapMacDinh", "MauHoaDonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoDieuChinhThongTinHoaDonChiTiet", b =>
                {
                    b.HasOne("DLL.Entity.DanhMuc.MauHoaDon", "MauHoaDon")
                        .WithMany("ThongBaoDieuChinhThongTinHoaDonChiTiets")
                        .HasForeignKey("MauHoaDonId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DLL.Entity.DanhMuc.ThongBaoDieuChinhThongTinHoaDon", "ThongBaoDieuChinhThongTinHoaDon")
                        .WithMany("ThongBaoDieuChinhThongTinHoaDonChiTiets")
                        .HasForeignKey("ThongBaoDieuChinhThongTinHoaDonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoKetQuaHuyHoaDonChiTiet", b =>
                {
                    b.HasOne("DLL.Entity.DanhMuc.MauHoaDon", "MauHoaDon")
                        .WithMany("ThongBaoKetQuaHuyHoaDonChiTiets")
                        .HasForeignKey("MauHoaDonId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DLL.Entity.DanhMuc.ThongBaoKetQuaHuyHoaDon", "ThongBaoKetQuaHuyHoaDon")
                        .WithMany("ThongBaoKetQuaHuyHoaDonChiTiets")
                        .HasForeignKey("ThongBaoKetQuaHuyHoaDonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DLL.Entity.DanhMuc.ThongBaoPhatHanhChiTiet", b =>
                {
                    b.HasOne("DLL.Entity.DanhMuc.MauHoaDon", "MauHoaDon")
                        .WithMany("ThongBaoPhatHanhChiTiets")
                        .HasForeignKey("MauHoaDonId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DLL.Entity.DanhMuc.ThongBaoPhatHanh", "ThongBaoPhatHanh")
                        .WithMany("ThongBaoPhatHanhChiTiets")
                        .HasForeignKey("ThongBaoPhatHanhId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DLL.Entity.Function_Role", b =>
                {
                    b.HasOne("DLL.Entity.Function", "Function")
                        .WithMany("Function_Roles")
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DLL.Entity.Permission", "Permission")
                        .WithMany("Function_Roles")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DLL.Entity.Role", "Role")
                        .WithMany("Function_Roles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DLL.Entity.Function_ThaoTac", b =>
                {
                    b.HasOne("DLL.Entity.Function", "Function")
                        .WithMany()
                        .HasForeignKey("FunctionId");

                    b.HasOne("DLL.Entity.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId");

                    b.HasOne("DLL.Entity.ThaoTac", "ThaoTac")
                        .WithMany()
                        .HasForeignKey("ThaoTacId");
                });

            modelBuilder.Entity("DLL.Entity.Function_User", b =>
                {
                    b.HasOne("DLL.Entity.Function", "Function")
                        .WithMany("Function_Users")
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DLL.Entity.Permission", "Permission")
                        .WithMany("Function_Users")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DLL.Entity.User", "User")
                        .WithMany("Function_Users")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DLL.Entity.User", b =>
                {
                    b.HasOne("DLL.Entity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DLL.Entity.User_Role", b =>
                {
                    b.HasOne("DLL.Entity.Role", "Role")
                        .WithMany("User_Roles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DLL.Entity.User", "User")
                        .WithMany("User_Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
