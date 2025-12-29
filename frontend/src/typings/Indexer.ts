import DownloadProtocol from 'DownloadClient/DownloadProtocol';
import Provider from './Provider';

interface Indexer extends Provider {
  enableRss: boolean;
  enableAutomaticSearch: boolean;
  enableInteractiveSearch: boolean;
  supportsRss: boolean;
  supportsSearch: boolean;
  protocol: DownloadProtocol;
  priority: number;
  isFallback: boolean;
  downloadClientId: number;
  tags: number[];
}

export default Indexer;
