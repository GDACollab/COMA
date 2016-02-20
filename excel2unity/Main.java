import java.io.File;
import java.io.PrintWriter;
import java.io.FileNotFoundException;
import java.io.BufferedReader;
import java.io.FileReader;
import java.util.Scanner;
import java.io.IOException;
import java.util.InputMismatchException;
import java.nio.file.Files;

class Main {
  public static void main(String[] args) {
    System.out.println("Running the converter");
    try {
      mmain(args);
    } catch(Exception e) {
      System.out.println(e);
    }
  }

  public static void mmain(String[] args) throws FileNotFoundException, IOException {
/*
    PrintWriter pw = new PrintWriter(new File());
    pw.println("Crap!");
    pw.close();
*/

    File dir = new File(".");
    File[] filesList = dir.listFiles();
    for (File file : filesList) {
      if (file.isFile() && file.getName().endsWith(".csv")) {
        String infilename = file.getName();
        String outfilename = infilename.substring(0,infilename.length()-4);
        outfilename += ".unity";
        BufferedReader br_out = new BufferedReader(new FileReader(new File(outfilename)));
        PrintWriter pw_temp = new PrintWriter(new File("temp"));

        String outline;
        int state = 0;
        while((outline = br_out.readLine()) != null) {
          if(state == 0) {
            pw_temp.print(outline + "\n");
            if(outline.equals("  triggers:")) {
              state = 1;

              Scanner scan_in = new Scanner(file);
              scan_in.useDelimiter(",");
              while(scan_in.hasNextDouble()) {
                double time = scan_in.nextDouble();
                String s = scan_in.nextLine().toUpperCase();
                char c = s.charAt(s.length()-1);
                if(c == ',')
                  continue;

                pw_temp.print("  - audioPos: " + time + "\n");
                pw_temp.print("    methodName: Maker" + c + "\n");
              }
              scan_in.close();
            }
          } else if(state == 1) {
            if(outline.charAt(0) != ' ') {
              state = 2;
              pw_temp.print(outline + "\n");
            }
          } else {
            pw_temp.print(outline + "\n");
          }
        }
        br_out.close();
        pw_temp.close();

        copyFile(new File("temp"), new File(outfilename));
      }
    }
  }

  static void copyFile( File from, File to ) throws IOException {
    Files.copy(from.toPath(), to.toPath(), java.nio.file.StandardCopyOption.REPLACE_EXISTING);
  }
}
