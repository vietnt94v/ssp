import ExcelJS from 'exceljs';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

export interface ExportColumn<T> {
  header: string;
  accessor: (row: T) => string | number;
}

function triggerDownload(blob: Blob, filename: string) {
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = filename;
  link.click();
  URL.revokeObjectURL(url);
}

export async function exportToExcel<T>(
  rows: T[],
  columns: ExportColumn<T>[],
  filename: string,
  sheetName = 'Sheet1',
) {
  const workbook = new ExcelJS.Workbook();
  const sheet = workbook.addWorksheet(sheetName);

  sheet.columns = columns.map((col) => ({
    header: col.header,
    key: col.header,
    width: 22,
  }));
  sheet.getRow(1).font = { bold: true };

  rows.forEach((row) => {
    sheet.addRow(columns.map((col) => col.accessor(row)));
  });

  const buffer = await workbook.xlsx.writeBuffer();
  triggerDownload(
    new Blob([buffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    }),
    `${filename}.xlsx`,
  );
}

export function exportToPdf<T>(
  rows: T[],
  columns: ExportColumn<T>[],
  filename: string,
  title?: string,
) {
  const doc = new jsPDF();
  if (title) {
    doc.setFontSize(14);
    doc.text(title, 14, 16);
  }
  autoTable(doc, {
    startY: title ? 22 : 14,
    head: [columns.map((col) => col.header)],
    body: rows.map((row) => columns.map((col) => String(col.accessor(row)))),
    styles: { fontSize: 8 },
    headStyles: { fillColor: [37, 99, 235] },
  });
  doc.save(`${filename}.pdf`);
}
