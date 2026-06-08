import { Download } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
  exportToExcel,
  exportToPdf,
  type ExportColumn,
} from '@/lib/export';

interface ExportButtonProps<T> {
  rows: T[];
  columns: ExportColumn<T>[];
  filename: string;
  title?: string;
}

export function ExportButton<T>({
  rows,
  columns,
  filename,
  title,
}: ExportButtonProps<T>) {
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="outline" size="sm">
          <Download className="mr-1 h-4 w-4" />
          Export
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuItem
          onClick={() => exportToExcel(rows, columns, filename)}
        >
          Export to Excel
        </DropdownMenuItem>
        <DropdownMenuItem
          onClick={() => exportToPdf(rows, columns, filename, title)}
        >
          Export to PDF
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
