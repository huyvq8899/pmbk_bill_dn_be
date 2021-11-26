﻿using ManagementServices.Helper;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text;

namespace Services.Helper
{
    public class ImageHelper
    {
        private static byte[] ImgBytes = new byte[] {   0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a, 0x00, 0x00, 0x00, 0x0d, 0x49, 0x48, 0x44, 0x52, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0xc5, 0x08, 0x03, 0x00,
                                                        0x00, 0x00, 0x8c, 0xfe, 0xdc, 0xe7, 0x00, 0x00, 0x00, 0x01, 0x73, 0x52, 0x47, 0x42, 0x00, 0xae, 0xce, 0x1c, 0xe9, 0x00, 0x00, 0x00, 0x04, 0x67, 0x41, 0x4d, 0x41,
                                                        0x00, 0x00, 0xb1, 0x8f, 0x0b, 0xfc, 0x61, 0x05, 0x00, 0x00, 0x00, 0xc6, 0x50, 0x4c, 0x54, 0x45, 0xff, 0xff, 0xff, 0x36, 0x8c, 0x1e, 0x4b, 0x9e, 0x30, 0x2b, 0x72,
                                                        0x14, 0x37, 0x8e, 0x1f, 0x00, 0x64, 0x00, 0x32, 0x8a, 0x18, 0x49, 0x9c, 0x2e, 0x00, 0x65, 0x00, 0x26, 0x87, 0x00, 0x3a, 0x8f, 0x22, 0x3f, 0x94, 0x26, 0x45, 0x99,
                                                        0x2b, 0xe7, 0xf0, 0xe5, 0x35, 0x8a, 0x1d, 0x4d, 0xa0, 0x32, 0x2c, 0x75, 0x15, 0x13, 0x81, 0x00, 0x31, 0x81, 0x1a, 0x2e, 0x7a, 0x17, 0x22, 0x6e, 0x00, 0x31, 0x80,
                                                        0x19, 0x33, 0x86, 0x1c, 0x17, 0x6a, 0x00, 0x26, 0x70, 0x0b, 0x2f, 0x89, 0x12, 0xf5, 0xf8, 0xf4, 0xd0, 0xdd, 0xcd, 0xc8, 0xd7, 0xc5, 0xe0, 0xeb, 0xde, 0x29, 0x88,
                                                        0x00, 0xca, 0xdd, 0xc6, 0xae, 0xcc, 0xa8, 0x90, 0xba, 0x87, 0x87, 0xb5, 0x7e, 0xae, 0xc4, 0xa9, 0x76, 0xab, 0x6b, 0x57, 0x9b, 0x47, 0xb5, 0xc9, 0xb0, 0x48, 0x94,
                                                        0x35, 0x74, 0x9c, 0x6b, 0x98, 0xbf, 0x90, 0xf1, 0xf6, 0xf0, 0x8c, 0xac, 0x84, 0xd9, 0xe3, 0xd6, 0x42, 0x7e, 0x32, 0x60, 0xa0, 0x52, 0xbd, 0xcf, 0xb9, 0x9f, 0xb9,
                                                        0x99, 0xa6, 0xc7, 0x9f, 0x55, 0x89, 0x48, 0x00, 0x74, 0x00, 0x73, 0xaa, 0x67, 0x88, 0xab, 0x80, 0x69, 0x98, 0x5e, 0xc5, 0xda, 0xc1, 0x14, 0x77, 0x00, 0x27, 0x80,
                                                        0x04, 0x5e, 0x8d, 0x53, 0x40, 0x7c, 0x30, 0x23, 0x74, 0x00, 0x37, 0x78, 0x24, 0x41, 0x91, 0x2c, 0x4c, 0x83, 0x3e, 0xb4, 0xc8, 0xaf, 0x87, 0xa9, 0x80, 0x24, 0xf1,
                                                        0x08, 0xf1, 0x00, 0x00, 0x0a, 0xde, 0x49, 0x44, 0x41, 0x54, 0x78, 0x5e, 0xe5, 0x5d, 0x6b, 0x7b, 0xd2, 0x4c, 0x10, 0x75, 0xd9, 0x90, 0x92, 0x14, 0x0a, 0xb1, 0x54,
                                                        0x08, 0x95, 0xb7, 0x17, 0xad, 0x17, 0xac, 0xb5, 0xad, 0xd5, 0xb6, 0xde, 0xfd, 0xff, 0x7f, 0xea, 0x9d, 0xd9, 0x9d, 0xdc, 0xef, 0x08, 0xdb, 0x84, 0x39, 0x5f, 0x4c,
                                                        0x10, 0x1f, 0x39, 0x9b, 0xcc, 0x9e, 0x99, 0x33, 0xc9, 0xee, 0x33, 0xe6, 0x58, 0xd2, 0x9f, 0x5c, 0x71, 0x4a, 0x7f, 0x32, 0xc5, 0xe2, 0x35, 0x1d, 0x30, 0xc5, 0xea,
                                                        0xed, 0x82, 0x8e, 0x78, 0x62, 0xe9, 0xad, 0xe8, 0x88, 0x27, 0x4e, 0x3e, 0x5f, 0xd2, 0x11, 0x4f, 0x9c, 0x7e, 0x7e, 0x49, 0x47, 0x3c, 0x71, 0xf6, 0xf9, 0x15, 0x1d,
                                                        0xf1, 0xc4, 0x9d, 0xf7, 0x85, 0x8e, 0x78, 0xe2, 0xc2, 0xfb, 0x4a, 0x47, 0x2c, 0xb1, 0x72, 0x9d, 0x8f, 0x74, 0xc8, 0x12, 0x4b, 0xc7, 0x3f, 0xe4, 0x9c, 0x00, 0xdc,
                                                        0x7b, 0xf6, 0x0f, 0xce, 0x09, 0xc0, 0x8d, 0x67, 0xb1, 0x4e, 0x00, 0xce, 0x81, 0xff, 0x27, 0x3a, 0xe6, 0x88, 0x3b, 0x47, 0x3c, 0x5c, 0xd3, 0x31, 0x43, 0x2c, 0xde,
                                                        0x3a, 0xe2, 0xf1, 0x2f, 0x9d, 0x30, 0xc4, 0xc8, 0xf2, 0xad, 0xc7, 0x6f, 0x74, 0xc2, 0x10, 0xc7, 0x8e, 0x6d, 0x3d, 0x7e, 0xa7, 0x13, 0x86, 0xb8, 0xf7, 0x84, 0x75,
                                                        0x30, 0xe5, 0x9b, 0x00, 0xdc, 0x20, 0xff, 0xc1, 0x88, 0xce, 0xf8, 0xe1, 0xdc, 0x13, 0x42, 0xf4, 0x8f, 0xe9, 0x8c, 0x1f, 0xae, 0x1c, 0xe0, 0x3f, 0x78, 0x43, 0x67,
                                                        0xec, 0xb0, 0x78, 0xeb, 0x0b, 0xcb, 0x9a, 0xb1, 0x4d, 0x00, 0x46, 0xbe, 0x6f, 0x59, 0xd6, 0xf4, 0x1d, 0x9d, 0xb2, 0xc3, 0xb1, 0x67, 0xbb, 0x42, 0xfc, 0xb8, 0xa5,
                                                        0x53, 0x76, 0x38, 0xf5, 0x84, 0x2b, 0xac, 0x1f, 0x3f, 0xe9, 0x94, 0x1d, 0xde, 0x2b, 0xfe, 0x87, 0x33, 0xae, 0x15, 0xf0, 0x6b, 0x07, 0xf9, 0x1f, 0xf5, 0xb9, 0xf6,
                                                        0x00, 0xaf, 0x1c, 0x4b, 0xf1, 0x67, 0x6a, 0x81, 0xaf, 0x7e, 0xf9, 0x2e, 0xcc, 0x7f, 0xcf, 0xfb, 0x4c, 0x2d, 0xf0, 0xe5, 0x7f, 0x36, 0xf0, 0xb7, 0x5e, 0x0c, 0x3e,
                                                        0xd0, 0x07, 0xcc, 0x70, 0xe2, 0xd9, 0xfb, 0x16, 0xf0, 0x9f, 0x31, 0xb5, 0xc0, 0x41, 0xfe, 0xf6, 0x05, 0xf0, 0x9f, 0xfe, 0xa6, 0x0f, 0x98, 0xe1, 0x4c, 0xf3, 0x3f,
                                                        0x98, 0x4b, 0xfa, 0x80, 0x19, 0xee, 0x1c, 0xe4, 0x2f, 0x84, 0x64, 0x9a, 0x00, 0x5c, 0x38, 0x96, 0xe2, 0x3f, 0xe9, 0xb3, 0xb4, 0xc0, 0x57, 0xae, 0xef, 0x12, 0x7f,
                                                        0x96, 0x16, 0xf8, 0xd2, 0xb1, 0x15, 0x7f, 0x6b, 0x32, 0x60, 0x59, 0x01, 0xdf, 0x83, 0xfc, 0xa9, 0xeb, 0x7f, 0x38, 0x63, 0x69, 0x81, 0xdf, 0xc0, 0xf4, 0x0f, 0xe9,
                                                        0x1f, 0x16, 0x40, 0x2c, 0x2d, 0xf0, 0x73, 0x4f, 0x0c, 0x35, 0xff, 0x29, 0xcb, 0x1e, 0xf8, 0x95, 0x23, 0x86, 0x78, 0xfb, 0x5b, 0x47, 0xf3, 0x39, 0x43, 0x0b, 0x1c,
                                                        0xcd, 0x3f, 0xe2, 0x2f, 0x39, 0x5a, 0xe0, 0x23, 0xdb, 0x77, 0x15, 0x7f, 0xf1, 0x5c, 0x72, 0xb4, 0xc0, 0x8f, 0x41, 0xfe, 0xf4, 0xf5, 0x7f, 0x21, 0xfb, 0x0c, 0x2d,
                                                        0x70, 0x94, 0xbf, 0x80, 0x3f, 0xc7, 0x04, 0xe0, 0x3d, 0xc8, 0x9f, 0x92, 0x7f, 0xe0, 0x3f, 0x63, 0x68, 0x81, 0xa3, 0xfc, 0x69, 0xfe, 0x07, 0x72, 0xca, 0xd0, 0x02,
                                                        0x47, 0xf9, 0x43, 0xf9, 0x47, 0xfe, 0x73, 0x7e, 0x16, 0x38, 0xca, 0xdf, 0x9e, 0xe2, 0x2f, 0x80, 0x3f, 0xbf, 0x1e, 0xf8, 0xd2, 0xf7, 0xad, 0x3d, 0x4b, 0xf1, 0x17,
                                                        0x13, 0x39, 0x60, 0x67, 0x81, 0x9f, 0x78, 0xb6, 0xbb, 0xa7, 0xe9, 0x5b, 0x13, 0xc9, 0xcf, 0x02, 0x47, 0xf3, 0x8f, 0xf8, 0xe3, 0xf5, 0x67, 0x67, 0x81, 0xa3, 0xfc,
                                                        0xe9, 0xf4, 0x0f, 0x0a, 0x20, 0x39, 0x63, 0x67, 0x81, 0xbf, 0x86, 0xe9, 0x3f, 0xc6, 0x9f, 0x5d, 0x02, 0x70, 0x01, 0xfc, 0x95, 0xfc, 0x03, 0xff, 0x23, 0xc9, 0xce,
                                                        0x02, 0x5f, 0xfd, 0x02, 0xf9, 0x8b, 0xf8, 0xb3, 0xb3, 0xc0, 0x97, 0xff, 0xf9, 0xa2, 0xa7, 0xe5, 0x5f, 0x58, 0xcf, 0xe5, 0x9c, 0x9b, 0x05, 0x0e, 0xd5, 0x8f, 0x1b,
                                                        0xf0, 0x87, 0x02, 0x58, 0x0e, 0x98, 0x59, 0xe0, 0xa7, 0x31, 0xfe, 0x50, 0x00, 0x49, 0x6e, 0x16, 0x38, 0xf6, 0xbe, 0x7a, 0x9a, 0xbe, 0xe2, 0xcf, 0xad, 0x02, 0xc6,
                                                        0xde, 0x57, 0x9c, 0x3f, 0x37, 0x0b, 0x1c, 0xe5, 0x2f, 0x48, 0xff, 0xa0, 0x00, 0x94, 0xcc, 0x7a, 0xe0, 0x23, 0x0b, 0xe4, 0x8f, 0xd2, 0x1f, 0x2c, 0x00, 0x25, 0x33,
                                                        0x0b, 0xfc, 0xd2, 0xb1, 0x45, 0x2f, 0xe4, 0x2f, 0xa4, 0x64, 0x66, 0x81, 0x83, 0xfc, 0x59, 0x63, 0x4a, 0x7f, 0x20, 0x00, 0x26, 0x10, 0x00, 0xac, 0x2c, 0xf0, 0x1b,
                                                        0x4f, 0xb8, 0x11, 0x7f, 0x28, 0x00, 0x99, 0x59, 0xe0, 0xe7, 0x20, 0x7f, 0xe3, 0x20, 0xfd, 0xc1, 0x02, 0x88, 0x59, 0x02, 0x70, 0x05, 0xf2, 0x97, 0xe4, 0xcf, 0x2a,
                                                        0x01, 0xc0, 0xf7, 0xbe, 0x86, 0x63, 0x62, 0xaf, 0xf9, 0xb3, 0xaa, 0x80, 0x47, 0xbe, 0x2f, 0x86, 0x41, 0xfa, 0x03, 0xfc, 0x8f, 0x40, 0x00, 0x39, 0xf5, 0xc0, 0x8f,
                                                        0x41, 0xfe, 0xf6, 0x82, 0xf4, 0x87, 0xf8, 0x73, 0x7a, 0x0d, 0x0c, 0xaa, 0x9f, 0xb8, 0xfc, 0x63, 0x01, 0xc8, 0xca, 0x02, 0xc7, 0x07, 0xdf, 0xc7, 0x11, 0x7f, 0x2c,
                                                        0x00, 0x58, 0xf5, 0xc0, 0xf1, 0xc1, 0xf7, 0x98, 0xfc, 0x2b, 0xfe, 0x9c, 0x12, 0x00, 0x25, 0x7f, 0x29, 0xfe, 0x83, 0x3f, 0xf4, 0x97, 0xbb, 0x0f, 0x34, 0xff, 0x12,
                                                        0xfc, 0xa1, 0x00, 0xe2, 0x64, 0x81, 0xa3, 0xf9, 0x37, 0x8c, 0xd2, 0x1f, 0x55, 0x00, 0x72, 0x4a, 0x00, 0x4e, 0x60, 0xfa, 0xdf, 0x4b, 0xf3, 0x67, 0xd4, 0x03, 0x3f,
                                                        0xf5, 0x84, 0xe8, 0x45, 0xe9, 0x0f, 0x00, 0x0a, 0x20, 0x46, 0x3d, 0x70, 0x94, 0xbf, 0x5e, 0x94, 0xfe, 0xc0, 0x04, 0x80, 0xfc, 0xf9, 0x58, 0xe0, 0x4a, 0xfe, 0xe2,
                                                        0xfc, 0xf1, 0xfa, 0x4b, 0x3e, 0x4f, 0x81, 0x5f, 0xa0, 0xfc, 0xc5, 0xd2, 0x3f, 0x55, 0x00, 0xf1, 0xe9, 0x81, 0xaf, 0x5c, 0x94, 0xbf, 0x0c, 0x7f, 0x36, 0x3d, 0xf0,
                                                        0x25, 0x54, 0x3f, 0xc3, 0x98, 0xfc, 0x13, 0x7f, 0x36, 0x16, 0xf8, 0x3d, 0xc8, 0x5f, 0x8a, 0x3f, 0x14, 0x80, 0x7c, 0x2c, 0x70, 0x94, 0xbf, 0x84, 0xfc, 0xab, 0x02,
                                                        0x90, 0x4f, 0x0f, 0xfc, 0xcc, 0x13, 0x56, 0x2f, 0xc9, 0x1f, 0x0b, 0x80, 0xb9, 0x64, 0x52, 0x01, 0xdf, 0xa1, 0xfc, 0xf5, 0xe8, 0xd9, 0x2f, 0x05, 0x55, 0x00, 0x19,
                                                        0x7f, 0x0d, 0x6c, 0xf9, 0x34, 0x6b, 0x0f, 0x2f, 0x94, 0xfc, 0x25, 0xd2, 0x3f, 0xcd, 0xdf, 0x74, 0x02, 0x30, 0x72, 0xbc, 0x13, 0x3a, 0x34, 0x09, 0xec, 0x7d, 0xed,
                                                        0x27, 0xd3, 0x1f, 0x55, 0x00, 0x1a, 0xb7, 0xc0, 0x47, 0xa0, 0x43, 0x9e, 0xf9, 0x07, 0x0f, 0xb1, 0xf7, 0x35, 0x4c, 0xc8, 0x3f, 0xf1, 0x37, 0x6d, 0x81, 0x23, 0x7f,
                                                        0x61, 0x3b, 0xa6, 0xd3, 0x6e, 0x2d, 0x7f, 0x09, 0xfe, 0xaa, 0x00, 0x94, 0x33, 0xc3, 0x4b, 0xa1, 0x22, 0x7f, 0xb8, 0x0d, 0xfd, 0xb7, 0x74, 0x6e, 0x08, 0x37, 0x4a,
                                                        0xfe, 0xe2, 0xf2, 0x0f, 0xc0, 0x02, 0xc0, 0xb4, 0x05, 0x8e, 0xfc, 0x7b, 0x18, 0x88, 0xfe, 0x15, 0x7d, 0x62, 0x04, 0xb8, 0xe8, 0x53, 0x2f, 0xc5, 0x1f, 0x0b, 0x40,
                                                        0xe3, 0x3d, 0x70, 0xc5, 0xbf, 0xa7, 0x46, 0xc0, 0x31, 0xb8, 0x0c, 0xfb, 0x15, 0xc8, 0x5f, 0x2e, 0x7f, 0xd3, 0x16, 0x38, 0xf1, 0x87, 0x11, 0x18, 0x1e, 0x08, 0xe7,
                                                        0x3d, 0x7d, 0xba, 0x6d, 0xe0, 0x83, 0xef, 0xee, 0x38, 0x99, 0xfe, 0xe8, 0x02, 0xc0, 0xb4, 0x05, 0x1e, 0xf2, 0x87, 0x11, 0x80, 0xcb, 0xe1, 0xdd, 0xd3, 0xe7, 0xdb,
                                                        0x05, 0xf6, 0xbe, 0x40, 0xfe, 0xf3, 0xf8, 0x1b, 0xb6, 0xc0, 0x63, 0xfc, 0x69, 0x04, 0x4c, 0xfc, 0xff, 0x68, 0xfe, 0x0d, 0x93, 0xe9, 0x4f, 0xc8, 0xdf, 0xec, 0x6b,
                                                        0x60, 0x09, 0xfe, 0x30, 0x02, 0x70, 0x49, 0x9c, 0xed, 0xdf, 0x81, 0x58, 0xfd, 0x0c, 0x93, 0xe9, 0x0f, 0xf0, 0xc7, 0x02, 0x50, 0x1a, 0x5e, 0x08, 0x25, 0xc5, 0x5f,
                                                        0x8d, 0x80, 0x6d, 0x6f, 0x7b, 0x0e, 0x7a, 0xaf, 0xe4, 0x2f, 0xcd, 0x1f, 0x0b, 0x40, 0xd3, 0x16, 0x78, 0x86, 0x3f, 0xc0, 0xda, 0x7a, 0x3a, 0xf0, 0xda, 0x41, 0xf9,
                                                        0x4b, 0xa6, 0x3f, 0xba, 0x00, 0x36, 0x6d, 0x81, 0xe7, 0xf1, 0x47, 0x5b, 0xda, 0xb9, 0xa3, 0x2f, 0x6c, 0x05, 0x5a, 0xfe, 0x52, 0xfc, 0x75, 0x01, 0x64, 0xd8, 0x02,
                                                        0xcf, 0xe7, 0x8f, 0xc6, 0xb4, 0x73, 0x4e, 0x5f, 0xd9, 0x3c, 0xb0, 0xf7, 0x05, 0xf2, 0x97, 0x4a, 0xff, 0x88, 0xbf, 0xd9, 0x0a, 0xb8, 0x80, 0xbf, 0x4a, 0x88, 0xbc,
                                                        0x1b, 0xfa, 0xd2, 0xa6, 0x81, 0xbd, 0xaf, 0xfd, 0x02, 0xfe, 0xd2, 0xec, 0x4a, 0x68, 0x45, 0xfc, 0x61, 0x04, 0xe0, 0xf6, 0xdc, 0x52, 0x3a, 0x80, 0xf2, 0x97, 0xc3,
                                                        0x5f, 0x15, 0x40, 0x86, 0x2d, 0x70, 0xe0, 0x6f, 0x11, 0xe1, 0x0c, 0x54, 0x3a, 0xb0, 0x8d, 0xab, 0xa1, 0xe5, 0x2f, 0x95, 0xfe, 0x01, 0x14, 0x7f, 0xb3, 0x16, 0x78,
                                                        0x19, 0x7f, 0x35, 0x02, 0xdb, 0xa8, 0x8d, 0xcf, 0x94, 0xfc, 0xa5, 0xd2, 0x3f, 0x80, 0xe2, 0x6f, 0xb6, 0x07, 0x5e, 0xce, 0x5f, 0x8f, 0x80, 0x4b, 0xdf, 0xdd, 0x18,
                                                        0x50, 0xfe, 0x80, 0x3f, 0xb1, 0x8e, 0x80, 0x05, 0x90, 0x61, 0x0b, 0xbc, 0x8a, 0x3f, 0x8c, 0x80, 0x2b, 0xfc, 0x0b, 0xfa, 0xf6, 0x86, 0x70, 0xe1, 0xa0, 0xf9, 0x9b,
                                                        0x4a, 0x7f, 0x60, 0x02, 0x50, 0xfc, 0xcd, 0x5a, 0xe0, 0xd5, 0xfc, 0x01, 0xd6, 0x66, 0xd3, 0x01, 0xec, 0x7d, 0x81, 0xfc, 0x65, 0xf9, 0xab, 0x02, 0xc0, 0xac, 0x05,
                                                        0x5e, 0x8b, 0xbf, 0x4a, 0x88, 0xce, 0xe8, 0x5f, 0xfc, 0x3b, 0xb0, 0xf7, 0x05, 0xd3, 0x7f, 0x3a, 0xfd, 0x23, 0xfe, 0x66, 0x2d, 0x70, 0xe0, 0xef, 0x12, 0xc7, 0x52,
                                                        0x60, 0x3a, 0xb0, 0x29, 0xab, 0x1c, 0xcd, 0xbf, 0x12, 0xfe, 0x46, 0x2d, 0xf0, 0xba, 0xfc, 0x75, 0x42, 0xb4, 0x19, 0xab, 0x1c, 0xcd, 0x3f, 0x90, 0xbf, 0x94, 0xfc,
                                                        0x03, 0x7f, 0x55, 0x00, 0x9a, 0xb5, 0xc0, 0x6b, 0xf3, 0x87, 0x11, 0xc0, 0x84, 0x68, 0x13, 0xe9, 0x00, 0x9a, 0x7f, 0x25, 0xfc, 0x8d, 0x5a, 0xe0, 0x0d, 0xf8, 0x6f,
                                                        0x2c, 0x1d, 0xb8, 0x53, 0xf2, 0x97, 0x4d, 0x7f, 0x74, 0x01, 0x68, 0xd6, 0x02, 0x6f, 0xc4, 0x5f, 0x8d, 0x80, 0xff, 0x8b, 0xfe, 0xe9, 0xba, 0xc0, 0xde, 0x17, 0xc8,
                                                        0x5f, 0x96, 0x3f, 0x15, 0x00, 0x46, 0x2d, 0x70, 0xe4, 0x3f, 0x26, 0x72, 0xb5, 0x80, 0x23, 0xf0, 0x6f, 0x56, 0xf9, 0xc8, 0x06, 0xf9, 0xeb, 0x25, 0x9b, 0x9f, 0x0a,
                                                        0x01, 0x7f, 0x93, 0x16, 0x78, 0x63, 0xfe, 0x30, 0x02, 0xee, 0xbf, 0x59, 0xe5, 0xd8, 0xfb, 0x82, 0xe9, 0x3f, 0x9b, 0xfe, 0x51, 0x01, 0x64, 0xd4, 0x02, 0x5f, 0x83,
                                                        0x3f, 0x00, 0x12, 0xa2, 0xf5, 0xad, 0xf2, 0x7b, 0x98, 0xfe, 0x80, 0x7f, 0x26, 0xfd, 0x09, 0xf8, 0x1b, 0xb5, 0xc0, 0xd7, 0xe3, 0x8f, 0x09, 0xd1, 0xda, 0xe9, 0x00,
                                                        0xc9, 0x5f, 0x46, 0xfe, 0xa9, 0x03, 0x08, 0x02, 0x68, 0xd0, 0x02, 0x57, 0xfc, 0xd7, 0x19, 0x80, 0xf5, 0xd3, 0x01, 0x92, 0xbf, 0x12, 0xfe, 0x26, 0x2d, 0x70, 0x14,
                                                        0x23, 0x77, 0x08, 0xd3, 0x51, 0x53, 0xa8, 0x84, 0x68, 0x9d, 0x3b, 0xf5, 0x4a, 0xcb, 0x5f, 0x0e, 0x7f, 0x5d, 0x00, 0x9a, 0xee, 0x81, 0x9f, 0xc3, 0xef, 0x81, 0x88,
                                                        0x84, 0x82, 0xb4, 0x21, 0x80, 0x82, 0xed, 0x37, 0x9e, 0xab, 0xb0, 0xf7, 0x25, 0x94, 0x8e, 0x64, 0xa0, 0x0b, 0x40, 0xf3, 0x4f, 0x81, 0x5f, 0x7b, 0x4a, 0x8b, 0x5c,
                                                        0xb8, 0x2b, 0x89, 0x5a, 0x4d, 0xa0, 0x18, 0x36, 0xb5, 0xca, 0xb1, 0xf7, 0x05, 0x29, 0x47, 0x1e, 0x7f, 0xba, 0xfe, 0x4f, 0xf0, 0x14, 0xf8, 0xa7, 0x07, 0x52, 0xe3,
                                                        0xfd, 0x86, 0x91, 0xd0, 0x3c, 0x1d, 0x40, 0xf3, 0x0f, 0xa6, 0xdc, 0x9c, 0xf4, 0x2f, 0x28, 0x80, 0x9e, 0xe4, 0x29, 0xf0, 0xe3, 0x07, 0x9b, 0x7e, 0x45, 0xc3, 0x48,
                                                        0x80, 0x11, 0x68, 0x64, 0x95, 0xa3, 0xf9, 0xb7, 0x0f, 0xff, 0x43, 0x09, 0xff, 0xa9, 0xe9, 0xc7, 0x51, 0x14, 0x46, 0x87, 0x7e, 0x90, 0x92, 0x35, 0x8b, 0x04, 0xb8,
                                                        0x94, 0x0d, 0xac, 0x72, 0xec, 0x7d, 0x21, 0xff, 0x4c, 0xfa, 0x17, 0xf2, 0x7f, 0xb2, 0x95, 0xd0, 0x16, 0x1f, 0x1f, 0xa3, 0x5f, 0xd5, 0x28, 0x12, 0xac, 0xfa, 0x56,
                                                        0x39, 0x9a, 0x7f, 0xc3, 0x71, 0xe2, 0xd1, 0xff, 0x00, 0x54, 0x00, 0x3e, 0xe5, 0x53, 0xe0, 0xdf, 0x1e, 0xe8, 0xb7, 0x20, 0x9a, 0xdc, 0x06, 0x20, 0x86, 0xf5, 0x7c,
                                                        0x0b, 0x92, 0xbf, 0x3c, 0xfe, 0x54, 0x00, 0x3e, 0xed, 0x53, 0xe0, 0x7f, 0x67, 0x22, 0x7e, 0x6b, 0xd6, 0xbe, 0x0d, 0xf6, 0xa0, 0x36, 0xae, 0x91, 0x0e, 0x60, 0xef,
                                                        0xab, 0x48, 0xfe, 0x03, 0xfe, 0x4f, 0xfc, 0x1a, 0xd8, 0x75, 0xff, 0x45, 0x22, 0x38, 0x6b, 0x4e, 0x88, 0x70, 0x49, 0x6d, 0xab, 0x32, 0x1d, 0x58, 0x3a, 0x5a, 0xfe,
                                                        0xf2, 0xf8, 0x53, 0x01, 0xf8, 0xf4, 0x4f, 0x81, 0x7f, 0xea, 0x1f, 0x25, 0xa7, 0xa7, 0x7a, 0x91, 0x00, 0xa4, 0x2a, 0xad, 0xf2, 0x40, 0xfe, 0xf2, 0xe4, 0x3f, 0xe0,
                                                        0xdf, 0x82, 0xa7, 0xc0, 0x2f, 0x67, 0xf3, 0x44, 0x18, 0x00, 0xea, 0x44, 0x02, 0x8a, 0x61, 0xb9, 0x55, 0x4e, 0xf2, 0x57, 0xca, 0xbf, 0x15, 0xaf, 0x81, 0xad, 0xe4,
                                                        0x34, 0x19, 0x06, 0x80, 0x1a, 0xb7, 0x01, 0x8e, 0x40, 0x99, 0x55, 0x8e, 0xbd, 0xaf, 0x42, 0xfe, 0x54, 0x00, 0xb5, 0xe5, 0x35, 0xb0, 0xdf, 0xb3, 0x43, 0xfa, 0x65,
                                                        0x11, 0xac, 0xca, 0x5a, 0x09, 0xd3, 0x81, 0xe2, 0xda, 0x18, 0xeb, 0x2d, 0x90, 0xbf, 0xbc, 0xf4, 0x2f, 0x2c, 0x00, 0xdb, 0xb3, 0x12, 0xda, 0xd7, 0xd9, 0xe4, 0x20,
                                                        0x7d, 0x13, 0x54, 0x4f, 0x88, 0x38, 0x02, 0x45, 0xe9, 0xc0, 0x85, 0xe6, 0x9f, 0x97, 0xfe, 0x01, 0x74, 0x01, 0xd0, 0xa6, 0x85, 0x50, 0x3e, 0x0c, 0x64, 0x26, 0x0c,
                                                        0x00, 0x55, 0x91, 0x00, 0x09, 0x51, 0x6e, 0x12, 0x87, 0xbd, 0x2f, 0xec, 0x7d, 0x66, 0xdd, 0x2f, 0x04, 0x15, 0x80, 0xed, 0x5a, 0x08, 0xe5, 0x55, 0x5f, 0x66, 0xc3,
                                                        0x00, 0x51, 0x3e, 0x21, 0xe6, 0x5b, 0xe5, 0xd8, 0xfb, 0xb2, 0xe0, 0x0e, 0xc9, 0x4d, 0x7f, 0x42, 0xfe, 0x2d, 0x7b, 0x0d, 0xec, 0xe5, 0x40, 0xe6, 0x85, 0x01, 0xa0,
                                                        0x2c, 0x12, 0x20, 0x1d, 0xc8, 0x5a, 0xe5, 0xd8, 0xfb, 0x42, 0xf9, 0x2f, 0xe0, 0x4f, 0x05, 0x40, 0xeb, 0x36, 0x03, 0x59, 0xce, 0xe6, 0xb9, 0x61, 0x00, 0x28, 0x36,
                                                        0x8f, 0x80, 0x64, 0x26, 0x1d, 0x40, 0xf3, 0x0f, 0xa7, 0xff, 0xdc, 0xf4, 0x2f, 0xe4, 0xdf, 0xc2, 0x95, 0xd0, 0x56, 0x3f, 0xa7, 0xf2, 0x30, 0x7f, 0x04, 0x8a, 0x6f,
                                                        0x03, 0xa0, 0x99, 0xb2, 0xca, 0xd1, 0xfc, 0xab, 0xe6, 0x3f, 0x9f, 0x98, 0xec, 0x81, 0xd7, 0xc5, 0xed, 0xac, 0x28, 0x0c, 0x00, 0x05, 0x13, 0x22, 0xa6, 0x03, 0x71,
                                                        0xab, 0x3c, 0x90, 0xbf, 0x3c, 0xf9, 0x07, 0xfe, 0x54, 0x00, 0xb6, 0x75, 0x25, 0xb4, 0x77, 0x33, 0x29, 0x9f, 0x17, 0x8d, 0x40, 0xc1, 0x84, 0x08, 0x54, 0xa3, 0x74,
                                                        0x00, 0x17, 0x7d, 0x2a, 0xe3, 0x4f, 0x05, 0x50, 0x7b, 0x17, 0x42, 0x01, 0x39, 0x2c, 0x0e, 0x03, 0x40, 0x5e, 0x24, 0x60, 0x3a, 0x40, 0x56, 0xf9, 0x48, 0x68, 0xf9,
                                                        0xcb, 0x4f, 0x7f, 0xc2, 0x02, 0xb0, 0xcd, 0x2b, 0xa1, 0xfd, 0xe9, 0xcb, 0x92, 0x30, 0x00, 0xe4, 0x44, 0xc2, 0xd8, 0xb5, 0xb5, 0x55, 0x8e, 0x8b, 0x3e, 0xa9, 0xe7,
                                                        0x0d, 0xf3, 0xf9, 0x07, 0x05, 0x40, 0xbb, 0x57, 0x42, 0x3b, 0x1e, 0xcc, 0x4b, 0xc3, 0x00, 0x90, 0x8d, 0x04, 0x4b, 0x59, 0xe5, 0xd8, 0xfb, 0x52, 0xfd, 0xe6, 0x0a,
                                                        0xfe, 0x2d, 0x5f, 0x08, 0x65, 0x34, 0x9d, 0x96, 0x87, 0x01, 0x20, 0x73, 0x1b, 0xa0, 0x55, 0x8e, 0xf2, 0xa7, 0xfa, 0x6d, 0xf4, 0xa5, 0x14, 0x42, 0xfe, 0xad, 0x5f,
                                                        0x08, 0x65, 0xf1, 0x1d, 0x46, 0xa0, 0x34, 0x0c, 0x10, 0xc9, 0xdb, 0x00, 0xd2, 0x01, 0x17, 0xa6, 0x3f, 0x94, 0xbf, 0xfc, 0xf4, 0x27, 0x2c, 0x00, 0x3b, 0xb1, 0x12,
                                                        0xda, 0x37, 0x10, 0x83, 0x8a, 0x30, 0x00, 0xc4, 0x27, 0x44, 0x4d, 0xbb, 0x58, 0xfe, 0x01, 0x9a, 0x7e, 0x47, 0x56, 0x42, 0x7b, 0x07, 0x62, 0x50, 0x15, 0x06, 0x80,
                                                        0x58, 0x24, 0xe0, 0x5b, 0x66, 0x28, 0x7f, 0x55, 0xfc, 0xbb, 0xb2, 0x19, 0xc8, 0x35, 0x88, 0x41, 0x75, 0x18, 0x00, 0xc2, 0x48, 0x18, 0xef, 0x97, 0xf1, 0x0f, 0x0a,
                                                        0xa0, 0xee, 0xac, 0x84, 0xf6, 0x09, 0x47, 0xa0, 0x3a, 0x0c, 0x00, 0xf1, 0xdb, 0x20, 0x3f, 0xfd, 0x89, 0xf1, 0xef, 0xd0, 0x4a, 0x68, 0x97, 0x28, 0x87, 0x35, 0xc2,
                                                        0x00, 0x10, 0x98, 0x47, 0x85, 0xfc, 0x05, 0x15, 0x00, 0xdd, 0x5a, 0x09, 0x6d, 0x34, 0x07, 0x31, 0x90, 0xb2, 0x46, 0x18, 0x00, 0x70, 0x42, 0x2c, 0x4a, 0xff, 0xc2,
                                                        0x02, 0xa8, 0x6b, 0x2b, 0xa1, 0x2d, 0x7e, 0xa3, 0x18, 0xd4, 0x0a, 0x03, 0x80, 0xab, 0x37, 0xbd, 0xca, 0x41, 0xc8, 0xbf, 0x7b, 0x2b, 0xa1, 0x7d, 0x55, 0x23, 0x70,
                                                        0x68, 0xd5, 0x1b, 0x82, 0x02, 0x84, 0x05, 0x60, 0x17, 0x57, 0x42, 0xfb, 0x8b, 0x72, 0x28, 0x8b, 0x7c, 0x92, 0x5a, 0x88, 0xf8, 0x77, 0x72, 0x25, 0x34, 0x25, 0x87,
                                                        0xb5, 0xc3, 0x20, 0x0f, 0x41, 0x01, 0xd8, 0xd5, 0xcd, 0x40, 0x5e, 0xea, 0x11, 0x58, 0x37, 0x0c, 0xc2, 0x02, 0xa0, 0xbb, 0x2b, 0xa1, 0x5d, 0xce, 0x50, 0x0e, 0x21,
                                                        0x0c, 0x88, 0x52, 0x23, 0x44, 0xfc, 0x3b, 0xbc, 0x12, 0xda, 0x4a, 0x2a, 0x39, 0x5c, 0x27, 0x0c, 0x22, 0xfe, 0xdd, 0x5e, 0x09, 0x4d, 0xcb, 0xa1, 0x3c, 0x4c, 0xf7,
                                                        0x50, 0xab, 0x10, 0x16, 0x80, 0x9d, 0x5f, 0x09, 0x0d, 0xad, 0x42, 0xc4, 0x01, 0x31, 0xab, 0x87, 0x88, 0x7f, 0xf7, 0x57, 0x42, 0x43, 0xab, 0x10, 0xd1, 0x28, 0x0c,
                                                        0x42, 0xfe, 0xbb, 0xb0, 0x12, 0xda, 0x2b, 0x2d, 0x06, 0x8d, 0xc2, 0x20, 0x28, 0x80, 0x76, 0x63, 0x33, 0x90, 0x97, 0xaa, 0x36, 0x02, 0xd4, 0x0d, 0x83, 0xb0, 0x00,
                                                        0xdc, 0x95, 0xa5, 0x50, 0x97, 0x53, 0x1a, 0x81, 0xd4, 0x13, 0x35, 0x45, 0x88, 0xf8, 0xb7, 0xd7, 0x02, 0x6f, 0x06, 0xec, 0x9c, 0x29, 0xd4, 0x09, 0x83, 0xb0, 0x00,
                                                        0x6a, 0xb7, 0x05, 0xde, 0x10, 0xb7, 0x24, 0x06, 0xd5, 0x49, 0x51, 0xc4, 0x7f, 0xb7, 0xd6, 0x02, 0x57, 0x56, 0x21, 0xa2, 0x22, 0x0c, 0xc2, 0x02, 0x68, 0xe7, 0x36,
                                                        0x03, 0xa1, 0xda, 0xa8, 0x22, 0x0c, 0x62, 0xfc, 0x77, 0x6e, 0x33, 0x90, 0x37, 0xc1, 0x08, 0x94, 0x85, 0x41, 0x58, 0x00, 0xee, 0xe2, 0x66, 0x20, 0xaa, 0x73, 0xa6,
                                                        0x50, 0x18, 0x06, 0x21, 0xff, 0xdd, 0xdc, 0x0c, 0x44, 0x75, 0xce, 0x14, 0x26, 0xb9, 0x61, 0x10, 0x15, 0x40, 0xbb, 0xba, 0x19, 0xc8, 0xe2, 0x63, 0x30, 0x02, 0x79,
                                                        0x61, 0x10, 0xe7, 0x6f, 0x76, 0x25, 0x34, 0x83, 0x50, 0x9d, 0x33, 0x85, 0x4c, 0x18, 0xc4, 0xf8, 0x1b, 0x5e, 0x09, 0xcd, 0x28, 0xbe, 0x04, 0x72, 0x98, 0x0e, 0x83,
                                                        0xa8, 0x00, 0xdc, 0xb1, 0x04, 0x20, 0x8d, 0x50, 0x0e, 0x53, 0x61, 0x10, 0xf1, 0xdf, 0xf5, 0xcd, 0x40, 0x74, 0xe7, 0x4c, 0x21, 0x1e, 0x06, 0x61, 0x01, 0xb0, 0xfb,
                                                        0x9b, 0x81, 0x04, 0x56, 0x21, 0x20, 0xec, 0xa1, 0x46, 0x05, 0x20, 0x87, 0xcd, 0x40, 0x56, 0x93, 0x50, 0x0c, 0x82, 0xbe, 0x41, 0xc8, 0xbf, 0x2b, 0x3d, 0xf0, 0x7f,
                                                        0x03, 0x75, 0xce, 0x14, 0x30, 0x0c, 0xa2, 0x02, 0x88, 0xcd, 0x66, 0x20, 0xba, 0x73, 0xa6, 0x00, 0x61, 0x10, 0xf1, 0x67, 0xb3, 0x19, 0x48, 0xd0, 0x39, 0x53, 0x88,
                                                        0xf8, 0x33, 0xda, 0x0d, 0x2c, 0xb4, 0x0a, 0xe3, 0xe0, 0xb5, 0x1f, 0x76, 0x68, 0x15, 0x86, 0x60, 0xb3, 0x19, 0x08, 0x21, 0x26, 0x87, 0x0a, 0x73, 0x49, 0x7f, 0xc1,
                                                        0x06, 0xa1, 0x55, 0xa8, 0xc1, 0x20, 0x01, 0xc8, 0x20, 0x26, 0x87, 0x8c, 0x76, 0x03, 0x8b, 0x23, 0xe8, 0x9c, 0xed, 0x90, 0x05, 0xde, 0x10, 0x41, 0xe7, 0x6c, 0x97,
                                                        0x2c, 0xf0, 0x66, 0xd0, 0x72, 0x68, 0x7a, 0x33, 0x90, 0x36, 0x01, 0xe5, 0x90, 0xd5, 0x7e, 0xd8, 0x19, 0x2c, 0xa7, 0xb3, 0xf6, 0xbd, 0x06, 0x66, 0x14, 0x8b, 0xdb,
                                                        0xce, 0x58, 0xe0, 0xcf, 0x9e, 0xfd, 0x0f, 0x07, 0x59, 0xc8, 0x8b, 0xa1, 0x0e, 0x27, 0xc1, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4e, 0x44, 0xae, 0x42, 0x60, 0x82};

        public static Bitmap CreateImageSignature(string TenP1, string TenP2, DateTime? signDate = null)
        {
            Bitmap bitmap = null;

            try
            {
                SizeF imageF = CalculateSizeImage(TenP1, TenP2);
                string sDate = signDate.HasValue ? signDate.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");

                // Get image tick
                Image i = BytesArrayToImage(ImgBytes);

                // Initialize new image from scratch
                bitmap = new Bitmap((int)imageF.Width + 10, (int)imageF.Height + 10, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.Clear(Color.FromKnownColor(KnownColor.White));

                // Initialize Brush class object
                Brush brush = new SolidBrush(Color.FromKnownColor(KnownColor.Black));
                Pen pen = new Pen(Color.FromKnownColor(KnownColor.Blue), 1);

                // Set font style, size, etc.
                Font arial = new Font("Times New Roman", 16, FontStyle.Regular);
                string measureString = string.Empty;
                if (!string.IsNullOrWhiteSpace(TenP2))
                {
                    measureString = $"Signature Valid\r\nKý bởi: {TenP1}\r\n{TenP2}\r\nKý ngày: {sDate}";
                }
                else
                {
                    measureString = $"Signature Valid\r\nKý bởi: {TenP1}\r\nKý ngày: {sDate}";
                }

                // Measure string.
                SizeF stringSize = new SizeF();
                stringSize = graphics.MeasureString(measureString, arial);
                graphics.DrawImage(i, (stringSize.Width - 30) / 2, 10, 90, stringSize.Height - 10);
                graphics.DrawRectangle(new Pen(Color.Green, 2), 5.0F, 5.0F, stringSize.Width, stringSize.Height);
                // Draw string
                graphics.DrawString(measureString, arial, Brushes.Green, new PointF(5, 5));
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return bitmap;
        }

        public static void AddSignatureImageToDoc(Document doc, string tenDonVi, DateTime? signDate = null)
        {
            var tenKySo = tenDonVi.GetTenKySo();
            var signatureImage = CreateImageSignature(tenKySo.Item1, tenKySo.Item2, signDate);

            TextSelection selection = doc.FindString("<digitalSignature>", true, true);
            if (selection != null)
            {
                DocPicture pic = new DocPicture(doc);
                pic.LoadImage(signatureImage);
                pic.Width = pic.Width * 48 / 100;
                pic.Height = pic.Height * 48 / 100;

                var range = selection.GetAsOneRange();
                var index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                range.OwnerParagraph.ChildObjects.Insert(index, pic);
                range.OwnerParagraph.ChildObjects.Remove(range);
            }
        }

        private static SizeF CalculateSizeImage(string TenP1, string TenP2)
        {
            // Initialize new image from scratch
            Bitmap bitmap = new Bitmap(1024, 960, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.Clear(Color.FromKnownColor(KnownColor.White));

            // Set font style, size, etc.
            Font roman = new Font("Times New Roman", 16, FontStyle.Regular);
            string measureString = string.Empty;
            if (!string.IsNullOrWhiteSpace(TenP2))
            {
                measureString = $"Signature Valid\r\nKý bởi: {TenP1}\r\n{TenP2}\r\nKý ngày: {DateTime.Now.ToString("yyyy-MM-dd")}";
            }
            else
            {
                measureString = $"Signature Valid\r\nKý bởi: {TenP1}\r\nKý ngày: {DateTime.Now.ToString("yyyy-MM-dd")}";
            }

            // Measure string.            
            return graphics.MeasureString(measureString, roman);
        }

        private static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            string base64String = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                string hex = BytesToHexStr(imageBytes);

                // Convert byte[] to base 64 string
                //base64String = Convert.ToBase64String(imageBytes);
            }

            return base64String;
        }

        private static Image BytesArrayToImage(byte[] bytes)
        {
            // Convert byte[] to Image
            using (var ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        private static string BytesToHexStr(byte[] buffer)
        {
            StringBuilder sbLogSource = new StringBuilder();
            for (int idx = 0; idx < buffer.Length; idx++)
            {
                sbLogSource.Append(String.Format("0x{0:x2}, ", buffer[idx]));
            }
            return (sbLogSource.ToString());
        }
    }
}
