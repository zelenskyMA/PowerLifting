import FileSaver from "file-saver";

export function SaveExcelFile(fileName, content) {
  const fileType = 'application/vnd.ms-excel';
  const nameWithExtension = fileName + '.xlsx';

  const byteChars = atob(content);
  const byteNumbers = new Array(byteChars.length);
  for (let i = 0; i < byteChars.length; i += 1) {
    byteNumbers[i] = byteChars.charCodeAt(i);
  }
  const byteArray = new Uint8Array(byteNumbers);

  const blob = new Blob([byteArray], { type: fileType });

  FileSaver.saveAs(blob, nameWithExtension);
}
