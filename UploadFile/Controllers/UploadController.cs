﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UploadFile.Models;

namespace UploadFile.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue, MultipartHeadersLengthLimit = Int32.MaxValue)]
        public IActionResult Upload(SingleFileModel model)
        {
            
            if (ModelState.IsValid)
            {
                model.IsResponse = true;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                // Criar pasta se não existir
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // Adquirir extensão do arquivo
                FileInfo fileInfo = new FileInfo(model.File.FileName);
                string fileName = model.FileName + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }

                model.IsSuccess = true;
                model.Message = "Arquivo enviado com sucesso";

            }
            return View("Index", model);
        }

        public IActionResult MultiFile()
        {
            MultipleFilesModel model = new MultipleFilesModel();
            return View(model);
        }

        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue)]
        public IActionResult MultiUpload(MultipleFilesModel model)
        {
            if (ModelState.IsValid)
            {
                model.IsResponse = true;
                if (model.Files.Count > 0)
                {
                    foreach (var file in model.Files)
                    {

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);


                        string fileNameWithPath = Path.Combine(path, file.FileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                    }
                    model.IsSuccess = true;
                    model.Message = "Arquivos enviados com sucesso";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "Selecione os arquivos";
                }
            }

            return View("MultiFile", model);
        }

    }
}