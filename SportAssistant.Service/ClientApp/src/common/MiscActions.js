import FileSaver from "file-saver";

export function SaveFile(fileName, content) {
/*  const byteNumbers = new Array(content.length);
  for (let i = 0; i < content.length; i += 1) {
    byteNumbers[i] = content.charCodeAt(i);
  }
  const byteArray = new Uint8Array(byteNumbers);*/
  const blob = new Blob([content]);

  FileSaver.saveAs(blob, fileName);
}
