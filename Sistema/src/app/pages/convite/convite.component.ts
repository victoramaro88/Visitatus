import { Component, OnInit } from '@angular/core';
import { ImportsModule } from '../../imports';
import { HttpService } from '../../services/http-service.service';
import { ActivatedRoute } from '@angular/router';
import { CryptoService } from '../../services/crypto.service';
import { Base64Service } from '../../services/base64.service';

@Component({
  selector: 'app-convite',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './convite.component.html',
  styleUrl: './convite.component.css'
})
export class ConviteComponent implements OnInit {

  boolLoading = true;

  parametroRota!: string | null;
  idSessaoCrypto: number = 0;

  constructor(
    private http: HttpService
    , private route: ActivatedRoute
    , private cryptoService: CryptoService
    , private base64Service: Base64Service
  ) { }

  ngOnInit(): void {
    try {
      // Capturar o parâmetro da rota (ex: /destino/42)
      this.parametroRota = this.route.snapshot.paramMap.get('data');
      // console.warn("Rota: ", this.parametroRota);

      let baseDecripto = this.cryptoService.decriptografar(this.parametroRota!);
      this.idSessaoCrypto = this.base64Service.decodeBase64ToNumber(baseDecripto);

      this.GetSessaoBySesCodi(this.idSessaoCrypto);

    } catch (error) {
      // this.mensagemRespCarteira = "Carteira Inválida!";
      this.boolLoading = false;
    }
  }

  GetSessaoBySesCodi(sesCodi: number) {
    this.boolLoading = true;
    try {
      this.http.GetSessaoBySesCodi(sesCodi).subscribe({
        next: (response) => {
          // this.lstSessao = response;
          console.warn("Convite Sessão:", response);
          this.GetTemplateLoja(response.LojCodi);
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
        }
      });
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      this.boolLoading = false;
    }
  }

  GetTemplateLoja(lojCodi: number) {
    try {
      this.http.GetTemplateLoja(lojCodi).subscribe({
        next: (response) => {
          // this.lstSessao = response;
          console.warn("Template do Convite:", response);
          this.boolLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar dados:', error);
          this.boolLoading = false;
        }
      });
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
      this.boolLoading = false;
    }
  }

}
