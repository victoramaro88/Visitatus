// Import PrimeNG modules
import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { CpfMaskPipe } from './services/cpf-mask.pipe';
import { BlockUIModule } from 'primeng/blockui';
import { CalendarModule } from 'primeng/calendar';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { InputSwitchModule } from 'primeng/inputswitch';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { TableModule } from 'primeng/table';
import { MenuModule } from 'primeng/menu';
import { TelefonePipe } from './services/telefone.pipe';
import { PasswordModule } from 'primeng/password';
import { InputMaskModule } from 'primeng/inputmask';
import { ImageModule } from 'primeng/image';
import { TooltipModule } from 'primeng/tooltip';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { RadioButtonModule } from 'primeng/radiobutton';

@NgModule({
  imports: [
    CommonModule,
    ButtonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxMaskDirective,
    CpfMaskPipe,
    BlockUIModule,
    CalendarModule,
    InputTextModule,
    DropdownModule,
    InputSwitchModule,
    FileUploadModule,
    ToastModule,
    TableModule,
    MenuModule,
    TelefonePipe,
    PasswordModule,
    InputMaskModule,
    ImageModule,
    TooltipModule,
    CardModule,
    DialogModule,
    RadioButtonModule
  ],
  exports: [
    CommonModule,
    ButtonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxMaskDirective,
    CpfMaskPipe,
    BlockUIModule,
    CalendarModule,
    InputTextModule,
    DropdownModule,
    InputSwitchModule,
    FileUploadModule,
    ToastModule,
    TableModule,
    MenuModule,
    TelefonePipe,
    PasswordModule,
    InputMaskModule,
    ImageModule,
    TooltipModule,
    CardModule,
    DialogModule,
    RadioButtonModule
  ],
  providers: [provideNgxMask()]
})
export class ImportsModule {}
